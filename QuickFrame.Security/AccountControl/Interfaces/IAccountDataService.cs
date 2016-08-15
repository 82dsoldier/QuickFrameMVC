using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;

namespace QuickFrame.Security.AccountControl.Interfaces {

	public interface IAccountDataService {

		IEnumerable<TAccount> GetUsers<TAccount>(string filter = "") where TAccount : IdentityUser;

		TAccount GetUser<TAccount>(string userId) where TAccount : IdentityUser;

		string BuildFilter(List<IdentityUser> inclusions = null, List<IdentityUser> exclusions = null);
	}
}