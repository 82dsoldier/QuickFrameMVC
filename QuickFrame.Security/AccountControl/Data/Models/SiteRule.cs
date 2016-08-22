using QuickFrame.Data.Models;
using System.Collections.Generic;

namespace QuickFrame.Security.AccountControl.Data.Models {

	public class SiteRule : DataModel {
		public string Url { get; set; }
		public int Priority { get; set; }
		public bool IsAllow { get; set; }
		public bool MatchPartial { get; set; }

		public virtual ICollection<GroupRule> GroupRules { get; set; }
		public virtual ICollection<SiteRole> SiteRoles { get; set; }
		public virtual ICollection<UserRule> UserRules { get; set; }
		public virtual ICollection<RoleRule> RoleRules { get; set; }
	}
}

// </auto-generated>