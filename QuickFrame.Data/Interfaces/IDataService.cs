using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace QuickFrame.Data.Interfaces {

	public interface IDataService<TEntity>
	: IDataServiceInt<TEntity>
	where TEntity : IDataModelInt {
	}

	public interface IDataService<TDataType, TEntity>
	: IReadOnlyDataService<TDataType, TEntity>
	where TEntity : IDataModel<TDataType> {
		TDataType Create(TEntity model);

		Task<TDataType> CreateAsync(TEntity model);

		TDataType Create<TModel>(TModel model);

		Task<TDataType> CreateAsync<TModel>(TModel model);

		bool Delete(TDataType id);

		void DeleteAsync(TDataType id);

		void Save(TEntity model);

		void SaveAsync(TEntity model);

		void Save<TModel>(TModel model);

		void SaveAsync<TModel>(TModel model);
	}

	public interface IDataService<T1, T2, TEntity> {

		void Create(TEntity model);

		void CreateAsync(TEntity model);

		void Create<TModel>(TModel model);

		void CreateAsync<TModel>(TModel model);

		bool Delete(T1 primaryId, T2 secondaryId);

		void DeleteAsync(T1 primaryId, T2 secondaryId);

		TEntity Get(T1 primaryId, T2 secondaryId);

		Task<TEntity> GetAsync(T1 primaryId, T2 secondaryId);

		TResult Get<TResult>(T1 primaryId, T2 secondaryId);

		Task<TResult> GetAsync<TResult>(T1 primaryId, T2 secondaryId);

		long GetCount();

		Task<long> GetCountAsync();

		IEnumerable<TEntity> GetList(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false);

		Task<IEnumerable<TEntity>> GetListAsync(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false);

		IEnumerable<TResult> GetList<TResult>(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false);

		Task<IEnumerable<TResult>> GetListAsync<TResult>(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false);

		void Save(TEntity model);

		void SaveAsync(TEntity model);

		void Save<TModel>(TModel model);

		void SaveAsync<TModel>(TModel model);
	}

	public interface IDataService<T1, T2, T3, TEntity> {

		void Create(TEntity model);

		void CreateAsync(TEntity model);

		void Create<TModel>(TModel model);

		void CreateAsync<TModel>(TModel model);

		bool Delete(T1 primaryId, T2 secondaryId, T3 thirdId);

		void DeleteAsync(T1 primaryId, T2 secondaryId, T3 thirdId);

		TEntity Get(T1 primaryId, T2 secondaryId, T3 thirdId);

		Task<TEntity> GetAsync(T1 primaryId, T2 secondaryId, T3 thirdId);

		TResult Get<TResult>(T1 primaryId, T2 secondaryId, T3 thirdId);

		Task<TResult> GetAsync<TResult>(T1 primaryId, T2 secondaryId, T3 thirdId);

		long GetCount();

		Task<long> GetCountAsync();

		IEnumerable<TEntity> GetList(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false);

		Task<IEnumerable<TEntity>> GetListAsync(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false);

		IEnumerable<TResult> GetList<TResult>(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false);

		Task<IEnumerable<TResult>> GetListAsync<TResult>(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false);

		void Save(TEntity model);

		void SaveAsync(TEntity model);

		void Save<TModel>(TModel model);

		void SaveAsync<TModel>(TModel model);
	}
}
