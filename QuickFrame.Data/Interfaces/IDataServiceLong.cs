using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace QuickFrame.Data.Interfaces
{

	public interface IDataServiceLong<TEntity>
		: IDataServiceCore<long, TEntity>
		where TEntity : IDataModelLong {
	}

}