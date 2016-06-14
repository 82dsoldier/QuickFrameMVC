using QuickFrame.Security.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Security.AccountControl.Interfaces
{
    public interface IAccountEnumeratorService<TAccount> 
		where TAccount : UserBase {
		IEnumerable<TAccount> GetUsers(string filter = "");
    }
}
