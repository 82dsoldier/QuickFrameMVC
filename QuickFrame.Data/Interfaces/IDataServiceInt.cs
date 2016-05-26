namespace QuickFrame.Data.Interfaces {

	public interface IDataServiceInt<TEntity>
		: IDataServiceCore<int, TEntity>
		where TEntity : IDataModelInt {
	}
}