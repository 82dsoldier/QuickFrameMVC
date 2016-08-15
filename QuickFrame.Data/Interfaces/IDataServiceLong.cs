namespace QuickFrame.Data.Interfaces {

	public interface IDataServiceLong<TEntity>
		: IDataService<long, TEntity>
		where TEntity : IDataModelLong {
	}
}