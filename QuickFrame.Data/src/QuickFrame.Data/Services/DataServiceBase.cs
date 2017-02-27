using ExpressMapper;
using QuickFrame.Data.Interfaces.Dtos;
using QuickFrame.Data.Interfaces.Models;
using QuickFrame.Data.Interfaces.Services;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System;

#if NETSTANDARD1_6
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
#else
using System.Data.Entity;
using System.Linq.Dynamic;
#endif

namespace QuickFrame.Data.Services {

	/// <summary>
	/// Provides functionality to read from and write to the database.
	/// </summary>
	/// <typeparam name="TContext">The ntity framework context used to access the database</typeparam>
	/// <typeparam name="TEntity">The type of entity being used by this service.</typeparam>
	public class DataServiceBase<TContext, TEntity>
		: IDataServiceBase<TEntity>
		where TContext : DbContext
		where TEntity : class {
		protected TContext _dbContext;

		/// <summary>
		/// The data service constructor.
		/// </summary>
		/// <param name="dbContext">Accepts a copy of the context being used for this service.</param>
		public DataServiceBase(TContext dbContext) {
			_dbContext = dbContext;
		}

		/// <summary>
		/// Creates a new entry of type TEntity.
		/// </summary>
		/// <param name="model">The model to be created.</param>
		public virtual void Create(TEntity model) {
			_dbContext.Set<TEntity>().Add(model);
			_dbContext.Entry(model).State = EntityState.Added;
			_dbContext.SaveChanges();
		}

		/// <summary>
		/// Creates a new entity in the database.  Converts the TModel to a TEntity before saving.
		/// </summary>
		/// <typeparam name="TModel">The type of model being passed in.</typeparam>
		/// <param name="model">The model to be created.</param>
		public virtual void Create<TModel>(TModel model) where TModel : IDataTransferObjectCore {
			Create(Mapper.Map<TModel, TEntity>(model));
		}

		/// <summary>
		/// Gets a count of all entities in the table.
		/// </summary>
		/// <param name="searchColumn">The column used to search the table.</param>
		/// <param name="searchTerm">The term used to search the searchColumn.</param>
		/// <param name="includeDeleted">True to include entities that have been deleted.</param>
		/// <returns>An integer indicating the number of records in the table.</returns>
		public virtual int GetCount(string searchColumn = "", string searchTerm = "", bool? isDeleted = false) {
			var query = _dbContext.Set<TEntity>().AsQueryable();
			if(!string.IsNullOrEmpty(searchTerm))
				query = _dbContext.Set<TEntity>().Where($"{searchColumn}.Contains(@0)", searchTerm);
			if(isDeleted != null)
				if(typeof(IDataModelDeletable).IsAssignableFrom(typeof(TEntity)))
					query = query.IsDeleted((bool)isDeleted);
			return query.Count();
		}

		public virtual int GetCount(string searchColumn, int searchTerm, bool? isDeleted = false) {
			var query = _dbContext.Set<TEntity>().Where($"{searchColumn} == @0", searchTerm);
			if(isDeleted != null)
				query = query.IsDeleted((bool)isDeleted);
			return query.Count();
		}

		/// <summary>
		/// Gets a list of items from the database table.
		/// </summary>
		/// <param name="includeDeleted">Set to true to include records marked as deleted</param>
		/// <returns>An enumerable list of objects matching the specified criteria.</returns>
		public virtual IEnumerable<TEntity> GetList(bool? isDeleted = false) =>
			GetList(0, 0, string.Empty, SortOrder.Ascending, string.Empty, string.Empty, isDeleted);

		/// <summary>
		/// Gets a list of items from the database table.
		/// </summary>
		/// <typeparam name="TResult">The type of object to return.</typeparam>
		/// <param name="includeDeleted">Set to true to include records marked as deleted</param>
		/// <returns>An enumerable list of objects matching the specified criteria.</returns>
		public virtual IEnumerable<TResult> GetList<TResult>(bool? isDeleted = false)
			=> Mapper.Map<IEnumerable<TEntity>, IEnumerable<TResult>>(GetList(0, 0, string.Empty, SortOrder.Ascending, string.Empty, string.Empty, isDeleted));

		/// <summary>
		/// Gets a list of items from the database table.
		/// </summary>
		/// <param name="page">The page on which to start.</param>
		/// <param name="itemsPerPage">The number of items to return.</param>
		/// <param name="includeDeleted">Set to true to include records marked as deleted</param>
		/// <returns>An enumerable list of objects matching the specified criteria.</returns>
		public virtual IEnumerable<TEntity> GetList(int page, int itemsPerPage, bool? isDeleted = false) =>
			GetList(page, itemsPerPage, string.Empty, SortOrder.Ascending, string.Empty, string.Empty, isDeleted);

		/// <summary>
		/// Gets a list of items from the database table.
		/// </summary>
		/// <typeparam name="TResult">The type of object to return.</typeparam>
		/// <param name="page">The page on which to start.</param>
		/// <param name="itemsPerPage">The number of items to return.</param>
		/// <param name="includeDeleted">Set to true to include records marked as deleted</param>
		/// <returns>An enumerable list of objects matching the specified criteria.</returns>
		public virtual IEnumerable<TResult> GetList<TResult>(int page, int itemsPerPage, bool? isDeleted = false)
			=> Mapper.Map<IEnumerable<TEntity>, IEnumerable<TResult>>(GetList(page, itemsPerPage, string.Empty, SortOrder.Ascending, string.Empty, string.Empty, isDeleted));

		/// <summary>
		/// Gets a list of items from the database table.
		/// </summary>
		/// <param name="searchTerm">The term, if any, for which to search.</param>
		/// <param name="searchColumn">The column, if any, to search.</param>
		/// <param name="includeDeleted">Set to true to include records marked as deleted</param>
		/// <returns>An enumerable list of objects matching the specified criteria.</returns>
		public virtual IEnumerable<TEntity> GetList(string searchTerm, string searchColumn, bool? isDeleted = false) =>
			GetList(0, 0, string.Empty, SortOrder.Ascending, searchTerm, searchColumn, isDeleted);

		/// <summary>
		/// Gets a list of items from the database table.
		/// </summary>
		/// <param name="searchTerm">The value, if any, for which to search.</param>
		/// <param name="searchColumn">The column, if any, to search.</param>
		/// <param name="includeDeleted">Set to true to include records marked as deleted</param>
		/// <returns>An enumerable list of objects matching the specified criteria.</returns>
		public virtual IEnumerable<TEntity> GetList(int? searchTerm, string searchColumn, bool? isDeleted = false) =>
			GetList(0, 0, string.Empty, SortOrder.Ascending, searchTerm, searchColumn, isDeleted);

		/// <summary>
		/// Gets a list of items from the database table.
		/// </summary>
		/// <typeparam name="TResult">The type of object to return.</typeparam>
		/// <param name="searchTerm">The term, if any, for which to search.</param>
		/// <param name="searchColumn">The column, if any, to search.</param>
		/// <param name="includeDeleted">Set to true to include records marked as deleted</param>
		/// <returns>An enumerable list of objects matching the specified criteria.</returns>
		public virtual IEnumerable<TResult> GetList<TResult>(string searchTerm, string searchColumn, bool? isDeleted = false) 
			=> Mapper.Map<IEnumerable<TEntity>, IEnumerable<TResult>>(GetList(0, 0, string.Empty, SortOrder.Ascending, searchTerm, searchColumn, isDeleted));

		/// <summary>
		/// Gets a list of items from the database table.
		/// </summary>
		/// <typeparam name="TResult">The type of object to return.</typeparam>
		/// <param name="searchTerm">The value, if any, for which to search.</param>
		/// <param name="searchColumn">The column, if any, to search.</param>
		/// <param name="includeDeleted">Set to true to include records marked as deleted</param>
		/// <returns>An enumerable list of objects matching the specified criteria.</returns>
		public virtual IEnumerable<TResult> GetList<TResult>(int? searchTerm, string searchColumn, bool? isDeleted = false)
			=> Mapper.Map<IEnumerable<TEntity>, IEnumerable<TResult>>(GetList(0, 0, string.Empty, SortOrder.Ascending, searchTerm, searchColumn, isDeleted));
		
		/// <summary>
		/// Gets a list of items from the database table.
		/// </summary>
		/// <param name="sortColumn">The column on which to sort and search.</param>
		/// <param name="sortOrder">The order to sort the items being returned.</param>
		/// <param name="includeDeleted">Set to true to include records marked as deleted</param>
		/// <returns>An enumerable list of objects matching the specified criteria.</returns>
		public virtual IEnumerable<TEntity> GetList(string sortColumn, SortOrder sortOrder, bool? isDeleted = false) =>
			GetList(0, 0, sortColumn, sortOrder, string.Empty, string.Empty, isDeleted);

		/// <summary>
		/// Gets a list of items from the database table.
		/// </summary>
		/// <typeparam name="TResult">The type of object to return.</typeparam>
		/// <param name="sortColumn">The column on which to sort and search.</param>
		/// <param name="sortOrder">The order to sort the items being returned.</param>
		/// <param name="includeDeleted">Set to true to include records marked as deleted</param>
		/// <returns>An enumerable list of objects matching the specified criteria.</returns>
		public virtual IEnumerable<TResult> GetList<TResult>(string sortColumn, SortOrder sortOrder, bool? isDeleted = false)
			=> Mapper.Map<IEnumerable<TEntity>, IEnumerable<TResult>>(GetList(0, 0, sortColumn, sortOrder, string.Empty, string.Empty, isDeleted));

		/// <summary>
		/// Gets a list of items from the database table.
		/// </summary>
		/// <param name="page">The page on which to start.</param>
		/// <param name="itemsPerPage">The number of items to return.</param>
		/// <param name="searchTerm">The term, if any, for which to search.</param>
		/// <param name="searchColumn">The column, if any, to search.</param>
		/// <param name="includeDeleted">Set to true to include records marked as deleted</param>
		/// <returns>An enumerable list of objects matching the specified criteria.</returns>
		public virtual IEnumerable<TEntity> GetList(int page, int itemsPerPage, string searchTerm, string searchColumn, bool? isDeleted = false) =>
			GetList(page, itemsPerPage, string.Empty, SortOrder.Ascending, searchTerm, searchColumn, isDeleted);

		/// <summary>
		/// Gets a list of items from the database table.
		/// </summary>
		/// <param name="page">The page on which to start.</param>
		/// <param name="itemsPerPage">The number of items to return.</param>
		/// <param name="searchTerm">The value, if any, for which to search.</param>
		/// <param name="searchColumn">The column, if any, to search.</param>
		/// <param name="includeDeleted">Set to true to include records marked as deleted</param>
		/// <returns>An enumerable list of objects matching the specified criteria.</returns>
		public virtual IEnumerable<TEntity> GetList(int page, int itemsPerPage, int? searchTerm, string searchColumn, bool? isDeleted = false) 
			=> GetList(page, itemsPerPage, string.Empty, SortOrder.Ascending, searchTerm, searchColumn, isDeleted);

		/// <summary>
		/// Gets a list of items from the database table.
		/// </summary>
		/// <typeparam name="TResult">The type of object to return.</typeparam>
		/// <param name="page">The page on which to start.</param>
		/// <param name="itemsPerPage">The number of items to return.</param>
		/// <param name="searchTerm">The term, if any, for which to search.</param>
		/// <param name="searchColumn">The column, if any, to search.</param>
		/// <param name="includeDeleted">Set to true to include records marked as deleted</param>
		/// <returns>An enumerable list of objects matching the specified criteria.</returns>
		public virtual IEnumerable<TResult> GetList<TResult>(int page, int itemsPerPage, string searchTerm, string searchColumn, bool? isDeleted = false) 
			=> Mapper.Map<IEnumerable<TEntity>, IEnumerable<TResult>>(GetList(page, itemsPerPage, string.Empty, SortOrder.Ascending, searchTerm, searchColumn, isDeleted));

		/// <summary>
		/// Gets a list of items from the database table.
		/// </summary>
		/// <typeparam name="TResult">The type of object to return.</typeparam>
		/// <param name="page">The page on which to start.</param>
		/// <param name="itemsPerPage">The number of items to return.</param>
		/// <param name="searchTerm">The value, if any, for which to search.</param>
		/// <param name="searchColumn">The column, if any, to search.</param>
		/// <param name="includeDeleted">Set to true to include records marked as deleted</param>
		/// <returns>An enumerable list of objects matching the specified criteria.</returns>
		public virtual IEnumerable<TResult> GetList<TResult>(int page, int itemsPerPage, int? searchTerm, string searchColumn, bool? isDeleted = false)
			=> Mapper.Map<IEnumerable<TEntity>, IEnumerable<TResult>>(GetList(page, itemsPerPage, string.Empty, SortOrder.Ascending, searchTerm, searchColumn, isDeleted));


		/// <summary>
		/// Gets a list of items from the database table.
		/// </summary>
		/// <typeparam name="TResult">The type of object to return.</typeparam>
		/// <param name="page">The page on which to start.</param>
		/// <param name="itemsPerPage">The number of items to return.</param>
		/// <param name="sortColumn">The column on which to sort and search.</param>
		/// <param name="sortOrder">The order to sort the items being returned.</param>
		/// <param name="includeDeleted">Set to true to include records marked as deleted</param>
		/// <returns>An enumerable list of objects matching the specified criteria.</returns>
		public virtual IEnumerable<TResult> GetList<TResult>(int page, int itemsPerPage, string sortColumn, SortOrder sortOrder, bool? isDeleted = false) 
			=> Mapper.Map<IEnumerable<TEntity>, IEnumerable<TResult>>(GetList(page, itemsPerPage, sortColumn, sortOrder, isDeleted));

		/// <summary>
		/// Gets a list of items from the database table.
		/// </summary>
		/// <param name="page">The page on which to start.</param>
		/// <param name="itemsPerPage">The number of items to return.</param>
		/// <param name="sortColumn">The column on which to sort and search.</param>
		/// <param name="sortOrder">The order to sort the items being returned.</param>
		/// <param name="includeDeleted">Set to true to include records marked as deleted</param>
		/// <returns>An enumerable list of objects matching the specified criteria.</returns>
		public virtual IEnumerable<TEntity> GetList(int page, int itemsPerPage, string sortColumn, SortOrder sortOrder, bool? isDeleted = false) {
			var query = _dbContext.Set<TEntity>().AsQueryable();

			if(typeof(IDataModelDeletable).IsAssignableFrom(typeof(TEntity)))
				if(isDeleted != null)
					query = query.IsDeleted((bool)isDeleted);

			if(!string.IsNullOrEmpty(sortColumn)) {
				if(sortOrder == SortOrder.Descending)
					query = query.OrderByDescending(sortColumn);
				else
					query = query.OrderBy(sortColumn);

				if(itemsPerPage > 0) {
					if(page > 1)
						query = query.Skip((page - 1) * itemsPerPage);
					query = query.Take(itemsPerPage);
				}
			}

			return query.AsNoTracking();
		}

		/// <summary>
		/// Gets a list of items from the database table.
		/// </summary>
		/// <param name="page">The page on which to start.</param>
		/// <param name="itemsPerPage">The number of items to return.</param>
		/// <param name="sortColumn">The column on which to sort and search.</param>
		/// <param name="sortOrder">The order to sort the items being returned.</param>
		/// <param name="searchTerm">The term, if any, for which to search.</param>
		/// <param name="searchColumn">The column, if any, to search.</param>
		/// <param name="includeDeleted">Set to true to include records marked as deleted</param>
		/// <returns>An enumerable list of objects matching the specified criteria.</returns>
		public virtual IEnumerable<TEntity> GetList(int page, int itemsPerPage, string sortColumn, SortOrder sortOrder, string searchTerm, string searchColumn, bool? isDeleted = false) {
			return GetList(page, itemsPerPage, sortColumn, sortOrder, searchTerm, searchColumn, isDeleted, true);
		}

		/// <summary>
		/// Gets a list of items from the database table.
		/// </summary>
		/// <param name="page">The page on which to start.</param>
		/// <param name="itemsPerPage">The number of items to return.</param>
		/// <param name="sortColumn">The column on which to sort and search.</param>
		/// <param name="sortOrder">The order to sort the items being returned.</param>
		/// <param name="searchTerm">The term, if any, for which to search.</param>
		/// <param name="searchColumn">The column, if any, to search.</param>
		/// <param name="includeDeleted">Set to true to include records marked as deleted</param>
		/// <returns>An enumerable list of objects matching the specified criteria.</returns>
		public virtual IEnumerable<TEntity> GetList(int page, int itemsPerPage, string sortColumn, SortOrder sortOrder, string searchTerm, string searchColumn, bool? isDeleted = false, bool matchPartial = true) {
			var query = GetList(page, itemsPerPage, sortColumn, sortOrder, isDeleted);

			if(!string.IsNullOrEmpty(searchTerm)) {
				if(string.IsNullOrEmpty(searchColumn))
					searchColumn = "Name";
				if(!searchColumn.Contains(",")) {
					query = _dbContext.Set<TEntity>().Where($"{searchColumn}.Contains(@0)", searchTerm);
				} else {
					var columns = searchColumn.Split(',');
					foreach(var column in columns) {
						query = _dbContext.Set<TEntity>().Where($"{column}.Contains(@0)", searchTerm);
					}
				}
			}

			return query;
		}
		/// <summary>
		/// Gets a list of items from the database table.
		/// </summary>
		/// <param name="page">The page on which to start.</param>
		/// <param name="itemsPerPage">The number of items to return.</param>
		/// <param name="sortColumn">The column on which to sort and search.</param>
		/// <param name="sortOrder">The order to sort the items being returned.</param>
		/// <param name="searchTerm">The value, if any, for which to search.</param>
		/// <param name="searchColumn">The column, if any, to search.</param>
		/// <param name="includeDeleted">Set to true to include records marked as deleted</param>
		/// <returns>An enumerable list of objects matching the specified criteria.</returns>
		public virtual IEnumerable<TEntity> GetList(int page, int itemsPerPage, string sortColumn, SortOrder sortOrder, int? searchTerm, string searchColumn, bool? isDeleted = false) {
			var query = GetList(page, itemsPerPage, sortColumn, sortOrder, isDeleted);

			if(string.IsNullOrEmpty(searchColumn))
				searchColumn = "Id";
			if(searchTerm == null)
				query = _dbContext.Set<TEntity>().Where($"{searchColumn} == NULL", searchTerm);
			else
				query = _dbContext.Set<TEntity>().Where($"{searchColumn} == (@0)", searchTerm);

			return query;
		}

		/// <summary>
		/// Gets a list of items from the database table.
		/// </summary>
		/// <typeparam name="TResult">The type of object to return.</typeparam>
		/// <param name="page">The page on which to start.</param>
		/// <param name="itemsPerPage">The number of items to return.</param>
		/// <param name="sortColumn">The column on which to sort and search.</param>
		/// <param name="sortOrder">The order to sort the items being returned.</param>
		/// <param name="searchTerm">The term, if any, for which to search.</param>
		/// <param name="searchColumn">The column, if any, to search.</param>
		/// <param name="includeDeleted">Set to true to include records marked as deleted</param>
		/// <returns>An enumerable list of objects matching the specified criteria.</returns>
		public virtual IEnumerable<TResult> GetList<TResult>(int page, int itemsPerPage, string sortColumn, SortOrder sortOrder, string searchTerm, string searchColumn, bool? isDeleted = false)
			=> Mapper.Map<IEnumerable<TEntity>, IEnumerable<TResult>>(GetList(page, itemsPerPage, sortColumn, sortOrder, searchTerm, searchColumn, isDeleted));

		/// <summary>
		/// Gets a list of items from the database table.
		/// </summary>
		/// <typeparam name="TResult">The type of object to return.</typeparam>
		/// <param name="page">The page on which to start.</param>
		/// <param name="itemsPerPage">The number of items to return.</param>
		/// <param name="sortColumn">The column on which to sort and search.</param>
		/// <param name="sortOrder">The order to sort the items being returned.</param>
		/// <param name="searchTerm">The value, if any, for which to search.</param>
		/// <param name="searchColumn">The column, if any, to search.</param>
		/// <param name="includeDeleted">Set to true to include records marked as deleted</param>
		/// <returns>An enumerable list of objects matching the specified criteria.</returns>
		public virtual IEnumerable<TResult> GetList<TResult>(int page, int itemsPerPage, string sortColumn, SortOrder sortOrder, int? searchTerm, string searchColumn, bool? isDeleted = false)
			=> Mapper.Map<IEnumerable<TEntity>, IEnumerable<TResult>>(GetList(page, itemsPerPage, sortColumn, sortOrder, searchTerm, searchColumn, isDeleted));

		/// <summary>
		/// Gets a list of items from the database table.
		/// </summary>
		/// <param name="searchTerm">The term for which to search.</param>
		/// <param name="page">The page on which to start.</param>
		/// <param name="itemsPerPage">The number of items to return.</param>
		/// <param name="sortColumn">The column on which to sort and search.</param>
		/// <param name="sortOrder">The order to sort the items being returned.</param>
		/// <param name="includeDeleted">True to include items marked as deleted.</param>
		/// <returns>An enumerable list of objects matching the specified criteria.</returns>
		[Obsolete("Use one of the new GetList overrides")]
		public virtual IEnumerable<TEntity> GetList(string searchTerm = "", int page = 1, int itemsPerPage = 25, string sortColumn = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false) {
			var query = default(IQueryable<TEntity>);

			if(string.IsNullOrEmpty(searchTerm))
				query = _dbContext.Set<TEntity>();
			else
				query = _dbContext.Set<TEntity>().Where($"{sortColumn}.Contains(@0)", searchTerm);

			if(!includeDeleted && typeof(IDataModelDeletable).IsAssignableFrom(typeof(TEntity)))
				query = query.IsNotDeleted();

			if(!string.IsNullOrEmpty(sortColumn)) {
				if(sortOrder == SortOrder.Descending)
					query = query.OrderByDescending(sortColumn);
				else
					query = query.OrderBy(sortColumn);
			}

			if(itemsPerPage > 0) {
				if(page > 1)
					query = query.Skip((page - 1) * itemsPerPage);
				query = query.Take(itemsPerPage);
			}

			return query.AsNoTracking();
		}
		
		/// <summary>
		/// Gets a list of items from the database table.
		/// </summary>
		/// <typeparam name="TResult">The type of entity to convert the models to before returning them.</typeparam>
		/// <param name="searchTerm">The term for which to search.</param>
		/// <param name="page">The page on which to start.</param>
		/// <param name="itemsPerPage">The number of items to return.</param>
		/// <param name="sortColumn">The column on which to sort and search.</param>
		/// <param name="sortOrder">The order to sort the items being returned.</param>
		/// <param name="includeDeleted">True to include items marked as deleted.</param>
		/// <returns>An IEnumerable of type TResult</returns>
		[Obsolete("Use one of the new GetList overrides")]
		public virtual IEnumerable<TResult> GetList<TResult>(string searchTerm = "", int page = 1, int itemsPerPage = 25, string sortColumn = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false)
			where TResult : IDataTransferObjectCore {
			foreach(var obj in GetList(searchTerm, page, itemsPerPage, sortColumn, sortOrder))
				yield return Mapper.Map<TEntity, TResult>(obj);
		}

		/// <summary>
		/// Saves a modified entity into the database.
		/// </summary>
		/// <param name="model">The model to save.</param>
		/// <remarks></remarks>
		public virtual void Save(TEntity model) {
			_dbContext.Set<TEntity>().Attach(model);
			_dbContext.Entry(model).State = EntityState.Modified;
			_dbContext.SaveChanges();
		}

		/// <summary>
		/// Saves a modified entity into the database after converting it to type TEntity.
		/// </summary>
		/// <typeparam name="TModel">The type of model being passed in.</typeparam>
		/// <param name="model">The model to be saved.</param>
		public virtual void Save<TModel>(TModel model) where TModel : IDataTransferObjectCore {
			Save(Mapper.Map<TModel, TEntity>(model));
		}
	}
}