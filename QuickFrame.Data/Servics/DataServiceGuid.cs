using ExpressMapper;
using QuickFrame.Data.Exceptions;
using QuickFrame.Data.Interfaces;
using QuickFrame.Di;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace QuickFrame.Data.Services {

	public abstract class DataServiceGuid<TContext, TEntity>
	: DataServiceCore<TContext, Guid, TEntity>
	where TContext : DbContext
	where TEntity : class, IDataModelGuid {

		public override bool Delete(Guid id) {
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

		public override TEntity Get(Guid id) {
			using(var contextFactory = ComponentContainer.Component<TContext>()) {
				return contextFactory.Component.Set<TEntity>().FirstOrDefault(obj => obj.Id == id);
			}
		}

		public override TResult Get<TResult>(Guid id) {
			using(var contextFactory = ComponentContainer.Component<TContext>()) {
				return Mapper.Map<TEntity, TResult>(contextFactory.Component.Set<TEntity>().FirstOrDefault(obj => obj.Id == id));
			}
		}
	}
}