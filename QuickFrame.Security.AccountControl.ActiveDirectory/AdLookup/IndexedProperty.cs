using QuickFrame.Security.AccountControl.ActiveDirectory.AdLookup.Interfaces;
using System.DirectoryServices;

namespace QuickFrame.Security.AccountControl.ActiveDirectory.AdLookup {

	public class IndexedProperty<T> where T : class {
		protected ResultPropertyValueCollection _searchResult;

		public T this[int i]
		{
			get
			{
				return _searchResult[i] as T;
			}
		}

		public int Count
		{
			get { return _searchResult.Count; }
		}

		public IndexedProperty(ResultPropertyValueCollection searchResult) {
			_searchResult = searchResult;
		}
	}

	public class IndexedProperty<TSrc, TDest>
		where TSrc : class
		where TDest : class {
		protected ResultPropertyValueCollection _searchResult;
		protected ITransformData<TSrc, TDest> _func = null;

		public TDest this[int i]
		{
			get
			{
				return _func.Call((_searchResult.Count > i ? _searchResult[i] : null) as TSrc);
			}
		}

		public IndexedProperty(ResultPropertyValueCollection searchResult, ITransformData<TSrc, TDest> func) {
			_searchResult = searchResult;
			_func = func;
		}
	}
}