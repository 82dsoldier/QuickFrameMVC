namespace QuickFrame.Security.AccountControl.Data.Models {

	public class UserRole {
		public string UserId { get; set; }
		public string RoleId { get; set; }

		public virtual SiteRole SiteRole { get; set; }
	}
}

// </auto-generated>