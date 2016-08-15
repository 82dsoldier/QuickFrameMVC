namespace QuickFrame.Security.AccountControl.Data.Models {

	public class SiteRole {
		public string Id { get; set; }
		public string Name { get; set; }
		public string NormalizedName { get; set; }
		public string ConcurrencyStamp { get; set; }
		public string Description { get; set; }

		public virtual System.Collections.Generic.ICollection<GroupRole> GroupRoles { get; set; }
		public virtual System.Collections.Generic.ICollection<SiteRoleClaim> SiteRoleClaims { get; set; }
		public virtual System.Collections.Generic.ICollection<SiteRule> SiteRules { get; set; }
		public virtual System.Collections.Generic.ICollection<UserRole> UserRoles { get; set; }
	}
}

// </auto-generated>