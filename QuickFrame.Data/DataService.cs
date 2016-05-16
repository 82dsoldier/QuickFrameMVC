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

namespace QuickFrame.Data {

	public class DataService<TContext, TEntity>
	: DataServiceInt<TContext, TEntity>
	where TContext : DbContext
	where TEntity : class, IDataModelInt {
	}
}