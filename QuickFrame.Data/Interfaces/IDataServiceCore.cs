using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace QuickFrame.Data.Interfaces {

	public interface IDataServiceCore<TDataType, TEntity>
		where TEntity : IDataModelCore<TDataType> {

		void Create(TEntity model);

		void CreateAsync(TEntity model);

		void Create<TModel>(TModel model) where TModel : IDataTransferObjectCore<TDataType, TEntity, TModel>;

		void CreateAsync<TModel>(TModel model) where TModel : IDataTransferObjectCore<TDataType, TEntity, TModel>;

		void Delete(int id);

		void DeleteAsync(int id);

		TEntity Get(int id);

		Task<TEntity> GetAsync(int id);

		TResult Get<TResult>(int id) where TResult : IDataTransferObjectCore<TDataType, TEntity, TResult>;

		Task<TResult> GetAsync<TResult>(int id) where TResult : IDataTransferObjectCore<TDataType, TEntity, TResult>;

		int GetCount(bool includeDeleted = false);

		Task<int> GetCountAsync(bool includeDeleted = false);

		IEnumerable<TEntity> GetList(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false);

		Task<IEnumerable<TEntity>> GetListAsync(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false);

		IEnumerable<TResult> GetList<TResult>(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false)
			where TResult : IDataTransferObjectCore<TDataType, TEntity, TResult>;

		Task<IEnumerable<TResult>> GetListAsync<TResult>(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false)
			where TResult : IDataTransferObjectCore<TDataType, TEntity, TResult>;

		void Save(TEntity model);

		void SaveAsync(TEntity model);

		void Save<TModel>(TModel model) where TModel : IDataTransferObjectCore<TDataType, TEntity, TModel>;

		void SaveAsync<TModel>(TModel model) where TModel : IDataTransferObjectCore<TDataType, TEntity, TModel>;
	}

	public interface IDataServiceInt<TEntity>
		: IDataServiceCore<int, TEntity>
		where TEntity : IDataModelInt {
	}

	public interface IDataServiceLong<TEntity>
		: IDataServiceCore<long, TEntity>
		where TEntity : IDataModelLong {
	}

	public interface IDataServiceGuid<TEntity>
	: IDataServiceCore<Guid, TEntity>
	where TEntity : IDataModelGuid {
	}

	public interface IDataService<TEntity>
	: IDataServiceCore<int, TEntity>
	where TEntity : IDataModelInt {
	}
}