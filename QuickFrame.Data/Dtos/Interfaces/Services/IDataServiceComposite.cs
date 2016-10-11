using QuickFrame.Data.Interfaces.Dtos;

namespace QuickFrame.Data.Interfaces.Services {

	public interface IDataServiceComposite<TEntity, TFirst, TSecond> : IDataServiceBase<TEntity> {

		void Delete(TFirst firstId, TSecond secondId);

		TEntity Get(TFirst firstId, TSecond secondId);

		TResult Get<TResult>(TFirst firstId, TSecond secondId) where TResult : IDataTransferObjectCore;
	}

	public interface IDataServiceComposite<TEntity, TFirst, TSecond, TThird> : IDataServiceBase<TEntity> {

		void Delete(TFirst firstId, TSecond secondId, TThird thirdId);

		TEntity Get(TFirst firstId, TSecond secondId, TThird thirdId);

		TResult Get<TResult>(TFirst firstId, TSecond secondId, TThird thirdId) where TResult : IDataTransferObjectCore;
	}
}