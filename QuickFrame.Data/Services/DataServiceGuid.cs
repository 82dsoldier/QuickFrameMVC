using ExpressMapper;
using QuickFrame.Data.Interfaces.Models;
using QuickFrame.Data.Interfaces.Services;
using System;
using System.Data.Entity;
using System.Linq;

namespace QuickFrame.Data.Services {

	public class DataServiceGuid<TContext, TEntity>
		: DataService<TContext, TEntity, Guid>
		, IDataServiceGuid<TEntity>
		where TContext : DbContext
		where TEntity : class, IDataModelGuid {

		public DataServiceGuid(TContext dbContext) : base(dbContext) {
		}

		public override void Delete(Guid id) {
			var model = _dbContext.Set<TEntity>().First(obj => obj.Id == id);
			if(typeof(IDataModelDeletable).IsAssignableFrom(typeof(TEntity))) {
				(model as IDataModelDeletable).IsDeleted = true;
			} else {
				_dbContext.Set<TEntity>().Remove(model);
			}
			_dbContext.SaveChanges();
		}

		public override TEntity Get(Guid id) {
			return _dbContext.Set<TEntity>().AsNoTracking().FirstOrDefault(obj => obj.Id == id);
		}

		public override TResult Get<TResult>(Guid id) {
			return Mapper.Map<TEntity, TResult>(_dbContext.Set<TEntity>().First(obj => obj.Id == id));
		}
	}
}