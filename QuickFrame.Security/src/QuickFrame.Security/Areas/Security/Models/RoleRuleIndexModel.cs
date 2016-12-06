using QuickFrame.Security.AccountControl.Models;
using System.Collections.Generic;

namespace QuickFrame.Security.Areas.Security.Models {

	public class RoleRuleIndexModel {
		public string RuleId { get; set; }
		public List<SiteRole> RoleList { get; } = new List<SiteRole>();
	}
}