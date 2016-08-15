namespace QuickFrame.Data.Interfaces {

	public interface IDataService<TEntity>
	: IDataServiceInt<TEntity>
	where TEntity : IDataModelInt {
	}

	public interface IDataService<TDataType, TEntity>
	: IGenericDataService<TDataType, TEntity>
	where TEntity : IDataModel<TDataType> {
	}
}