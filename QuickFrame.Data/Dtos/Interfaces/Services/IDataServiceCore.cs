using QuickFrame.Data.Interfaces.Dtos;

namespace QuickFrame.Data.Interfaces.Services {

	public interface IDataServiceCore<TEntity, TIdType> : IDataServiceBase<TEntity> {

		void Delete(TIdType id);

		TEntity Get(TIdType id);

		TResult Get<TResult>(TIdType id) where TResult : IDataTransferObjectCore;
	}
}