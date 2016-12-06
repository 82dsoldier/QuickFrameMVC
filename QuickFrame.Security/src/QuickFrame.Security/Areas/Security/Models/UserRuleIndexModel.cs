using QuickFrame.Security.AccountControl.Models;
using System.Collections.Generic;

namespace QuickFrame.Security.Areas.Security.Models {

	public class UserRuleIndexModel {
		public int RuleId { get; set; }
		public List<SiteUser> UserList { get; set; } = new List<SiteUser>();
	}
}