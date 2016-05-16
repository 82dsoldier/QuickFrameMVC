using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace QuickFrame.Data.Interfaces
{

	public interface IDataServiceInt<TEntity>
		: IDataServiceCore<int, TEntity>
		where TEntity : IDataModelInt {
	}

}