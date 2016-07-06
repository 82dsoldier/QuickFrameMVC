using ExpressMapper;
using QuickFrame.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Data.Dtos
{
	public class ReadOnlyDataTransferObject<TSrc, TDest> : GenericDataTransferObject<TSrc, TDest>, IReadOnlyDataTransferObject<TSrc, TDest> {

    }
}
