using QuickFrame.Data.Interfaces.Dtos;
using QuickFrame.Data.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Data.Dtos
{
    public class ConcurrentDataTransferObject<TSrc, TDest> 
		: ConcurrentDataTransferObjectInt<TSrc, TDest>, IConcurrentDataTransferObject
		where TSrc : class, IConcurrentDataModelInt
    {
    }
}
