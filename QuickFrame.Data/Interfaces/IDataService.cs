using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace QuickFrame.Data.Interfaces
{

	public interface IDataService<TEntity>
	: IDataServiceInt<TEntity>
	where TEntity : IDataModelInt {
	}

}