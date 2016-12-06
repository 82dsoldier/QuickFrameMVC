using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Data.Interfaces.Models
{
    public interface IConcurrentDataModelCore : IDataModelCore
    {
		byte[] RowVersion { get; set; }
	}
}
