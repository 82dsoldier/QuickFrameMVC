using QuickFrame.Data.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace QuickFrame.Data.Interfaces {

	public interface IDataServiceCore<TDataType, TEntity> 
		: IGenericDataService<TDataType, TEntity> 
		where TEntity : IDataModelCore<TDataType> {
	}
}