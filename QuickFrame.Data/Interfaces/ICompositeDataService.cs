using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace QuickFrame.Data.Interfaces {

	public interface ICompositeDataService<TPrimaryDataType, TSecondaryDataType, TEntity> {

		void Create(TEntity model);

		void CreateAsync(TEntity model);

		void Create<TModel>(TModel model);

		void CreateAsync<TModel>(TModel model);

		bool Delete(TPrimaryDataType primaryId, TSecondaryDataType secondaryId);

		void DeleteAsync(TPrimaryDataType primaryId, TSecondaryDataType secondaryId);

		TEntity Get(TPrimaryDataType primaryId, TSecondaryDataType secondaryId);

		Task<TEntity> GetAsync(TPrimaryDataType primaryId, TSecondaryDataType secondaryId);

		TResult Get<TResult>(TPrimaryDataType primaryId, TSecondaryDataType secondaryId);

		Task<TResult> GetAsync<TResult>(TPrimaryDataType primaryId, TSecondaryDataType secondaryId);

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