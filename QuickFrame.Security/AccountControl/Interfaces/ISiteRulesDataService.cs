using QuickFrame.Security.AccountControl.Data.Models;
using System.Collections.Generic;

namespace QuickFrame.Security.AccountControl.Interfaces {

	public interface ISiteRulesDataService {

		IEnumerable<SiteRule> GetSiteRulesForUser(string userId);

		IEnumerable<SiteRule> GetSiteRulesForRole(string roleId);

		IEnumerable<SiteRule> GetSiteRulesForGroup(string groupId);
	}
}