using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace QuickFrame.Data.Interfaces {

	public interface IDataServiceCore<TDataType, TEntity> where TEntity : IDataModelCore<TDataType> {

		void Create(TEntity model);

		void CreateAsync(TEntity model);

		void Create<TModel>(TModel model)
			where TModel : IDataTransferObjectCore<TDataType, TEntity, TModel>;

		void CreateAsync<TModel>(TModel model)
			where TModel : IDataTransferObjectCore<TDataType, TEntity, TModel>;

		void Delete(TDataType id);

		void DeleteAsync(TDataType id);

		long GetCount(bool includeDeleted = false);
		Task<long> GetCountAsync(bool includeDeleted = false);
		TEntity Get(TDataType id);

		Task<TEntity> GetAsync(TDataType id);
		TResult Get<TResult>(TDataType id) where TResult : IDataTransferObjectCore<TDataType, TEntity, TResult>;

		Task<TResult> GetAsync<TResult>(TDataType id) where TResult : IDataTransferObjectCore<TDataType, TEntity, TResult>;

		IEnumerable<TEntity> GetList(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false);
		IEnumerable<TResult> GetList<TResult>(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false)
			where TResult : IDataTransferObjectCore<TDataType, TEntity, TResult>;

		Task<IEnumerable<TResult>> GetListAsync<TResult>(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false)
			where TResult : IDataTransferObjectCore<TDataType, TEntity, TResult>;
		void Save(TEntity model);

		void SaveAsync(TEntity model);
		void Save<TModel>(TModel model)
			where TModel : IDataTransferObjectCore<TDataType, TEntity, TModel>;

		void SaveAsync<TModel>(TModel model)
			where TModel : IDataTransferObjectCore<TDataType, TEntity, TModel>;

	}

}