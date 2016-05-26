namespace QuickFrame.Data.Interfaces {

	public interface IDataService<TEntity>
	: IDataServiceInt<TEntity>
	where TEntity : IDataModelInt {
	}
}