using QuickFrame.Data.Interfaces.Dtos;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace QuickFrame.Data.Interfaces.Services {

	public interface IDataServiceBase<TEntity> {

		void Create(TEntity model);

		void Create<TModel>(TModel model) where TModel : IDataTransferObjectCore;

		int GetCount();

		IEnumerable<TEntity> GetList(string searchTerm, int page, int itemsPerPage, string sortColumn, SortOrder sortOrder, bool includeDeleted);

		IEnumerable<TResult> GetList<TResult>(string searchTerm, int page, int itemsPerPage, string sortColumn, SortOrder sortOrder, bool includeDeleted) where TResult : IDataTransferObjectCore;

		void Save(TEntity model);

		void Save<TModel>(TModel model) where TModel : IDataTransferObjectCore;
	}
}