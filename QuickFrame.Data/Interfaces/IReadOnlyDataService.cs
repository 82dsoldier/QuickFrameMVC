using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Data.Interfaces
{
    public interface IReadOnlyDataService<TDataType, TEntity> {
		TEntity Get(TDataType id);

		Task<TEntity> GetAsync(TDataType id);

		TResult Get<TResult>(TDataType id);

		Task<TResult> GetAsync<TResult>(TDataType id);

		long GetCount();

		Task<long> GetCountAsync();

		IEnumerable<TEntity> GetList(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false);

		Task<IEnumerable<TEntity>> GetListAsync(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false);

		IEnumerable<TResult> GetList<TResult>(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false);

		Task<IEnumerable<TResult>> GetListAsync<TResult>(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false);
	}
}
