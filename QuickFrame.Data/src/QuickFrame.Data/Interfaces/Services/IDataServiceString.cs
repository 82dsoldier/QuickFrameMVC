using QuickFrame.Data.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Data.Interfaces.Services
{
    public interface IDataServiceString<TEntity>
		: IDataService<TEntity, string>
		where TEntity : class, IDataModelString {

	}
}
