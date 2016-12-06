using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace QuickFrame.Security.AccountControl.Models {

	public class SiteUser : IdentityUser {
		public string DisplayName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}
}