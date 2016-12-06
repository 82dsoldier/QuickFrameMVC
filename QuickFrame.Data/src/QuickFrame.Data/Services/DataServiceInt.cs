using ExpressMapper;
using QuickFrame.Data.Interfaces.Models;
using QuickFrame.Data.Interfaces.Services;
#if NETSTANDARD1_6
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
#endif
using System.Linq;
using System.Reflection;

namespace QuickFrame.Data.Services {

	public class DataServiceInt<TContext, TEntity>
		: DataService<TContext, TEntity, int>
		, IDataServiceInt<TEntity>
		where TContext : DbContext
		where TEntity : class, IDataModelInt {

		public DataServiceInt(TContext dbContext) : base(dbContext) {
		}

		public override void Delete(int id) {
			var model = _dbContext.Set<TEntity>().First(obj => obj.Id == id);
			if(typeof(IDataModelDeletable).GetTypeInfo().IsAssignableFrom(typeof(TEntity))) {
				(model as IDataModelDeletable).IsDeleted = true;
			} else {
				_dbContext.Set<TEntity>().Remove(model);
			}
			_dbContext.SaveChanges();
		}

		public override TEntity Get(int id) {
			return _dbContext.Set<TEntity>().AsNoTracking().FirstOrDefault(obj => obj.Id == id);
		}

		public override TResult Get<TResult>(int id) {
			return Mapper.Map<TEntity, TResult>(_dbContext.Set<TEntity>().First(obj => obj.Id == id));
		}
	}
}