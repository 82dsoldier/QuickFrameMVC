using QuickFrame.Security.AccountControl.Data.Dtos;
using QuickFrame.Security.AccountControl.Data.Models;
using System.Collections.Generic;

namespace QuickFrame.Security.Areas.Security.Models {

	public class GroupRuleIndexModel {
		public int RuleId { get; set; }
		public List<SiteGroup> GroupList { get; set; } = new List<SiteGroup>();
	}
}