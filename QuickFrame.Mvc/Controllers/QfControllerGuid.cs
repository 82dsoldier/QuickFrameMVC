using QuickFrame.Data.Interfaces.Dtos;
using QuickFrame.Data.Interfaces.Services;
using QuickFrame.Security;

namespace QuickFrame.Mvc.Controllers {

	public class QfControllerInt<TEntity, TIndex>
		: QfControllerCore<TEntity, int, TIndex>
		where TEntity : class, new()
		where TIndex : class, IDataTransferObject<int> {

		public QfControllerInt(IDataServiceCore<TEntity, int> dataService, QuickFrameSecurityManager securityManager)
			: base(dataService, securityManager) {
		}
	}

	public class QfControllerInt<TEntity, TIndex, TEdit>
		: QfControllerCore<TEntity, int, TIndex, TEdit>
		where TEntity : class, new()
		where TIndex : class, IDataTransferObject<int>
		where TEdit : class, IDataTransferObject<int> {

		public QfControllerInt(IDataServiceCore<TEntity, int> dataService, QuickFrameSecurityManager securityManager)
			: base(dataService, securityManager) {
		}
	}
}