using System.Collections.Generic;

namespace QuickFrame.Security.AccountControl.Data.Models {

	public class SiteRole {
		public string Id { get; set; }
		public string Name { get; set; }
		public string NormalizedName { get; set; }
		public string ConcurrencyStamp { get; set; }
		public string Description { get; set; }

		public virtual ICollection<GroupRole> GroupRoles { get; set; }
		public virtual ICollection<SiteRoleClaim> SiteRoleClaims { get; set; }
		public virtual ICollection<SiteRule> SiteRules { get; set; }
		public virtual ICollection<UserRole> UserRoles { get; set; }
		public virtual ICollection<RoleRule> RoleRules { get; set; }
	}
}

// </auto-generated>