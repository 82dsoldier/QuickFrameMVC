using ExpressMapper;
using QuickFrame.Data.Interfaces.Dtos;
using QuickFrame.Data.Interfaces.Models;
using QuickFrame.Data.Interfaces.Services;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;

namespace QuickFrame.Data.Services {

	public class DataServiceBase<TContext, TEntity>
		: IDataServiceBase<TEntity>
		where TContext : DbContext
		where TEntity : class {
		protected TContext _dbContext;

		public DataServiceBase(TContext dbContext) {
			_dbContext = dbContext;
		}

		public virtual void Create(TEntity model) {
			_dbContext.Set<TEntity>().Add(model);
			_dbContext.SaveChanges();
		}

		public virtual void Create<TModel>(TModel model) where TModel : IDataTransferObjectCore {
			Create(Mapper.Map<TModel, TEntity>(model));
		}

		public virtual int GetCount(string searchColumn = "", string searchTerm = "") {
			var query = _dbContext.Set<TEntity>().AsQueryable();
			if(!string.IsNullOrEmpty(searchTerm))
				query = _dbContext.Set<TEntity>().Where($"{searchColumn}.Contains(@0)", searchTerm);
			return query.Count();
		}

		public virtual IEnumerable<TEntity> GetList(string searchTerm = "", int page = 1, int itemsPerPage = 25, string sortColumn = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false) {
			var query = default(IQueryable<TEntity>);

			if(string.IsNullOrEmpty(searchTerm))
				query = _dbContext.Set<TEntity>();
			else
				query = _dbContext.Set<TEntity>().Where($"{sortColumn}.Contains(@0)", searchTerm);

			if(!string.IsNullOrEmpty(sortColumn)) {
				if(sortOrder == SortOrder.Descending)
					query = query.OrderByDescending(sortColumn);
				else
					query = query.OrderBy(sortColumn);

				if(!includeDeleted && typeof(IDataModelDeletable).IsAssignableFrom(typeof(TEntity)))
					query = query.IsNotDeleted();

				if(itemsPerPage > 0) {
					if(page > 1)
						query = query.Skip((page - 1) * itemsPerPage);
					query = query.Take(itemsPerPage);
				}
			}

			return query.AsNoTracking();
		}

		public virtual IEnumerable<TResult> GetList<TResult>(string searchTerm = "", int page = 1, int itemsPerPage = 25, string sortColumn = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false)
			where TResult : IDataTransferObjectCore {
			foreach(var obj in GetList(searchTerm, page, itemsPerPage, sortColumn, sortOrder))
				yield return Mapper.Map<TEntity, TResult>(obj);
		}

		public virtual void Save(TEntity model) {
			_dbContext.Set<TEntity>().AddOrUpdate(model);
			_dbContext.SaveChanges();
		}

		public virtual void Save<TModel>(TModel model) where TModel : IDataTransferObjectCore {
			Save(Mapper.Map<TModel, TEntity>(model));
		}
	}
}