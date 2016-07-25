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

	public abstract class DataServiceInt<TContext, TEntity>
	   : DataServiceCore< TContext, int,TEntity>
	   where TContext : DbContext
	   where TEntity : class, IDataModelInt {

		public override int Create(TEntity model) {
			using(var contextFactory = ComponentContainer.Component<TContext>()) {
				contextFactory.Component.Set<TEntity>().Add(model);
				contextFactory.Component.SaveChanges();
				return model.Id;
			}
		}
		public override bool Delete(int id) {
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

		public override TEntity Get(int id) {
			using(var contextFactory = ComponentContainer.Component<TContext>()) {
				return contextFactory.Component.Set<TEntity>().FirstOrDefault(obj => obj.Id == id);
			}
		}

		public override TResult Get<TResult>(int id) {
			using(var contextFactory = ComponentContainer.Component<TContext>()) {
				var app = contextFactory.Component.Set<TEntity>().FirstOrDefault(obj => obj.Id == id);
				return Mapper.Map<TEntity, TResult>(app);
			}
		}
	}
}