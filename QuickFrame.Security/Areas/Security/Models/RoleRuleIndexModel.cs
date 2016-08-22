using QuickFrame.Security.AccountControl.Data.Dtos;
using QuickFrame.Security.AccountControl.Data.Models;
using System.Collections.Generic;

namespace QuickFrame.Security.Areas.Security.Models {

	public class RoleRuleIndexModel {
		public string RuleId { get; set; }
		public List<SiteRole> RoleList { get; } = new List<SiteRole>();
	}
}