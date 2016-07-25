using ExpressMapper;
using QuickFrame.Data.Exceptions;
using QuickFrame.Data.Interfaces;
using QuickFrame.Di;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System;

namespace QuickFrame.Data.Services {

	public abstract class DataServiceLong<TContext, TEntity>
	: DataServiceCore<TContext, long, TEntity>
	where TContext : DbContext
	where TEntity : class, IDataModelLong {

		public override long Create(TEntity model) {
			using(var contextFactory = ComponentContainer.Component<TContext>()) {
				contextFactory.Component.Set<TEntity>().Add(model);
				contextFactory.Component.SaveChanges();
				return model.Id;
			}
		}
		public override bool Delete(long id) {
			using(var contextFactory = ComponentContainer.Component<TContext>()) {
				var dbModel = contextFactory.Component.Set<TEntity>().FirstOrDefault(obj => obj.Id == id);
				if(dbModel == null)
					throw new RecordDoesNotExistException();
				dbModel.IsDeleted = true;
				contextFactory.Component.Entry(dbModel).State = EntityState.Modified;
				contextFactory.Component.SaveChanges();
			}
			return true;
		}

		public override TEntity Get(long id) {
			using(var contextFactory = ComponentContainer.Component<TContext>()) {
				return contextFactory.Component.Set<TEntity>().FirstOrDefault(obj => obj.Id == id);
			}
		}

		public override TResult Get<TResult>(long id) {
			using(var contextFactory = ComponentContainer.Component<TContext>()) {
				return Mapper.Map<TEntity, TResult>(contextFactory.Component.Set<TEntity>().FirstOrDefault(obj => obj.Id == id));
			}
		}
	}
}