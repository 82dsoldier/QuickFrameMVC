using ExpressMapper;
using QuickFrame.Data.Exceptions;
using QuickFrame.Data.Interfaces;
using QuickFrame.Di;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ExpressMapper.Extensions;

namespace QuickFrame.Data
{

	public class DataServiceInt<TContext, TEntity>
		: DataServiceCore<int, TContext, TEntity>
		where TContext : DbContext
		where TEntity : class, IDataModelInt {
		public override void Delete(int id) {
			using (var contextFactory = ComponentContainer.Component<TContext>()) {
				var dbModel = contextFactory.Component.Set<TEntity>().FirstOrDefault(obj => obj.Id == id);
				if (dbModel == null)
					throw new RecordDoesNotExistException();
				dbModel.IsDeleted = true;
				contextFactory.Component.Entry(dbModel).State = EntityState.Modified;
				contextFactory.Component.SaveChanges();
			}
		}

		public override TEntity Get(int id) {
			using (var contextFactory = ComponentContainer.Component<TContext>()) {
				return contextFactory.Component.Set<TEntity>().FirstOrDefault(obj => obj.Id == id);
			}
		}

		public override TResult Get<TResult>(int id) {
			using (var contextFactory = ComponentContainer.Component<TContext>()) {
				return Mapper.Map<TEntity, TResult>(contextFactory.Component.Set<TEntity>().FirstOrDefault(obj => obj.Id == id));
			}
		}
	}

}