namespace QuickFrame.Data.Interfaces {

	public interface IDataServiceInt<TEntity>
		: IDataService<int, TEntity>
		where TEntity : IDataModelInt {
	}
}