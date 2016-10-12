using QuickFrame.Data.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Data.Models
{
    public class ConcurrentDataModelInt
		: ConcurrentDataModel<int>, IConcurrentDataModelInt
    {
    }
}
