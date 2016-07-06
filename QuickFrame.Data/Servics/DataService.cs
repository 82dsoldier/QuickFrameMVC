using QuickFrame.Data.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;

namespace QuickFrame.Data.Services {

	public abstract class DataService<TContext, TEntity>
	: DataServiceInt<TContext, TEntity>
	where TContext : DbContext
	where TEntity : class, IDataModelInt { 
	}
}