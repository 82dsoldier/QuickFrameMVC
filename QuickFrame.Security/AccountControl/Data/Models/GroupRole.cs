namespace QuickFrame.Security.AccountControl.Data.Models {

	public class GroupRole {
		public string RoleId { get; set; }
		public string GroupId { get; set; }

		public virtual SiteRole SiteRole { get; set; }
	}
}

// </auto-generated>