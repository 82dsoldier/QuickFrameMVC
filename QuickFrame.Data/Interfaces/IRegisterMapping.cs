using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Data.Interfaces
{
    public interface IRegisterMapping<TSrc, TDest>
    {
		void Register();
    }
}
