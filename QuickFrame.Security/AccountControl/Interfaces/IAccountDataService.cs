using QuickFrame.Security.AccountControl.Data.Models;
using QuickFrame.Security.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Security.AccountControl.Interfaces
{
    public interface IAccountDataService {
		IEnumerable<TAccount> GetUsers<TAccount>(string filter = "") where TAccount : UserBase;
		TAccount GetUser<TAccount>(string userId) where TAccount : UserBase;
		string BuildFilter(List<UserBase> inclusions = null, List<UserBase> exclusions = null);
	}
}
