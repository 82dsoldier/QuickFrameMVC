using ExpressMapper;
using QuickFrame.Data.Interfaces.Models;
using QuickFrame.Data.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
#if NETSTANDARD1_6
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
#endif

namespace QuickFrame.Data.Services
{
	public class DataServiceString<TContext, TEntity>
		: DataService<TContext, TEntity, string>
		, IDataServiceString<TEntity>
		where TContext : DbContext
		where TEntity : class, IDataModelString {
		public DataServiceString(TContext dbContext) : base(dbContext) {
		}

		public override void Delete(string id) {
			var model = _dbContext.Set<TEntity>().First(obj => obj.Id == id);
			if(typeof(IDataModelDeletable).GetTypeInfo().IsAssignableFrom(typeof(TEntity))) {
				(model as IDataModelDeletable).IsDeleted = true;
			} else {
				_dbContext.Set<TEntity>().Remove(model);
			}
			_dbContext.SaveChanges();
		}

		public override TEntity Get(string id) {
			return _dbContext.Set<TEntity>().AsNoTracking().FirstOrDefault(obj => obj.Id == id);
		}

		public override TResult Get<TResult>(string id) {
			return Mapper.Map<TEntity, TResult>(_dbContext.Set<TEntity>().First(obj => obj.Id == id));
		}
	}
}
