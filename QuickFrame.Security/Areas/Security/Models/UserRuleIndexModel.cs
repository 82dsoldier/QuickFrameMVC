using QuickFrame.Security.AccountControl.Data.Dtos;
using QuickFrame.Security.AccountControl.Data.Models;
using System.Collections.Generic;

namespace QuickFrame.Security.Areas.Security.Models {

	public class UserRuleIndexModel {
		public int RuleId { get; set; }
		public List<SiteUser> UserList { get; set; } = new List<SiteUser>();
	}
}
