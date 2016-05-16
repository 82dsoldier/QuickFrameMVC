using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace QuickFrame.Data.Interfaces
{

	public interface IDataServiceGuid<TEntity>
	: IDataServiceCore<Guid, TEntity>
	where TEntity : IDataModelGuid {
	}

}