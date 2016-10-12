using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Data.Interfaces.Models
{
    public interface IConcurrentDataModel<TIdType> : IDataModel<TIdType>, IConcurrentDataModelCore
    {
    }

	public interface IConcurrentDataModel : IDataModel, IConcurrentDataModelCore {

	}
}
