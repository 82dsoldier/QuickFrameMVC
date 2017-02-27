using QuickFrame.Data.Interfaces.Dtos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace QuickFrame.Data.Interfaces.Services {

	public interface IDataServiceBase<TEntity> {

		void Create(TEntity model);

		void Create<TModel>(TModel model) where TModel : IDataTransferObjectCore;

		int GetCount(string searchColumn = "", string searchTerm = "", bool? isDeleted = false);
		int GetCount(string searchColumn, int searchTerm, bool? isDeleted = false);
		[Obsolete("Use one of the new GetList overrides")]
		IEnumerable<TEntity> GetList(string searchTerm, int page, int itemsPerPage, string sortColumn, SortOrder sortOrder, bool includeDeleted);
		[Obsolete("Use one of the new GetList overrides")]
		IEnumerable<TResult> GetList<TResult>(string searchTerm, int page, int itemsPerPage, string sortColumn, SortOrder sortOrder, bool isDeleted) where TResult : IDataTransferObjectCore;
		IEnumerable<TEntity> GetList(int page, int itemsPerPage, string sortColumn, SortOrder sortOrder, string searchTerm, string searchColumn, bool? isDeleted = false);
		IEnumerable<TEntity> GetList(int page, int itemsPerPage, string sortColumn, SortOrder sortOrder, string searchTerm, string searchColumn, bool? isDeleted = false, bool matchPartial = true);
		IEnumerable<TEntity> GetList(int page, int itemsPerPage, bool? isDeleted = false);
		IEnumerable<TEntity> GetList(bool? isDeleted = false);
		IEnumerable<TEntity> GetList(string searchTerm, string searchColumn, bool? isDeleted = false);
		IEnumerable<TEntity> GetList(int? searchTerm, string searchColumn, bool? isDeleted = false);
		IEnumerable<TEntity> GetList(int page, int itemsPerPage, string searchTerm, string searchColumn, bool? isDeleted = false);
		IEnumerable<TEntity> GetList(int page, int itemsPerPage, int? searchTerm, string searchColumn, bool? isDeleted = false);
		IEnumerable<TEntity> GetList(string sortColumn, SortOrder sortOrder, bool? isDeleted = false);
		IEnumerable<TEntity> GetList(int page, int itemsPerPage, string sortColumn, SortOrder sortOrder, bool? isDeleted = false);
		IEnumerable<TEntity> GetList(int page, int itemsPerPage, string sortColumn, SortOrder sortOrder, int? searchTerm, string searchColumn, bool? isDeleted = false);
		IEnumerable<TResult> GetList<TResult>(int page, int itemsPerPage, string sortColumn, SortOrder sortOrder, string searchTerm, string searchColumn, bool? isDeleted = false);
		IEnumerable<TResult> GetList<TResult>(int page, int itemsPerPage, bool? isDeleted = false);
		IEnumerable<TResult> GetList<TResult>(bool? isDeleted = false);
		IEnumerable<TResult> GetList<TResult>(string searchTerm, string searchColumn, bool? isDeleted = false);
		IEnumerable<TResult> GetList<TResult>(int? searchTerm, string searchColumn, bool? isDeleted = false);
		IEnumerable<TResult> GetList<TResult>(int page, int itemsPerPage, string searchTerm, string searchColumn, bool? isDeleted = false);
		IEnumerable<TResult> GetList<TResult>(int page, int itemsPerPage, int? searchTerm, string searchColumn, bool? isDeleted = false);
		IEnumerable<TResult> GetList<TResult>(string sortColumn, SortOrder sortOrder, bool? isDeleted = false);
		IEnumerable<TResult> GetList<TResult>(int page, int itemsPerPage, string sortColumn, SortOrder sortOrder, bool? isDeleted = false);
		IEnumerable<TResult> GetList<TResult>(int page, int itemsPerPage, string sortColumn, SortOrder sortOrder, int? searchTerm, string searchColumn, bool? isDeleted = false);
		void Save(TEntity model);

		void Save<TModel>(TModel model) where TModel : IDataTransferObjectCore;
	}
}