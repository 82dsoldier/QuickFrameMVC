using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Data.Interfaces.Dtos
{
    public interface IConcurrentDataTransferObjectCore : IDataTransferObjectCore
    {
		byte[] RowVersion { get; set; }

	}
}
