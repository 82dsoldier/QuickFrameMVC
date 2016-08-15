using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace QuickFrame.Security.AccountControl.Data.Models {

	public class SiteUser : IdentityUser {
		public string DisplayName { get; set; }
	}
}