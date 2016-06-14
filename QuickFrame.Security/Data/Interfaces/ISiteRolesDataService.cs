using QuickFrame.Data.Interfaces;
using QuickFrame.Security.Data.Models;
using System.Collections.Generic;

namespace QuickFrame.Security.Data.Interfaces {

	public interface ISiteRolesDataService : IDataService<SiteRole> {
		IEnumerable<string> GetRolesForUser(string userId);
		IEnumerable<string> GetRolesForGroup(string groupId);
	}
}