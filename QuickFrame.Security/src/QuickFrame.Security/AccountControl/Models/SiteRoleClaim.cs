namespace QuickFrame.Security.AccountControl.Models {

	public class SiteRoleClaim {
		public int Id { get; set; }
		public string RoleId { get; set; }
		public string ClaimType { get; set; }
		public string ClaimValue { get; set; }

		public virtual SiteRole SiteRole { get; set; }
	}
}

// </auto-generated>