using ExpressMapper;
using QuickFrame.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Data.Dtos
{
	public class GenericDataTransferObject<TSrc, TDest> : IGenericDataTransferObject<TSrc, TDest> {
		public virtual void Register() {
			Mapper.Register<TSrc, TDest>();
			Mapper.Register<TDest, TSrc>();
		}
    }
}
