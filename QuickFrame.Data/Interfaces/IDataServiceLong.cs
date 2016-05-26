namespace QuickFrame.Data.Interfaces {

	public interface IDataServiceLong<TEntity>
		: IDataServiceCore<long, TEntity>
		where TEntity : IDataModelLong {
	}
}