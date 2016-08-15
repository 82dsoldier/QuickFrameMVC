namespace QuickFrame.Security.AccountControl.Data.Models {

	public class UserRule {
		public int RuleId { get; set; }
		public string UserId { get; set; }

		public virtual SiteRule SiteRule { get; set; }
	}
}