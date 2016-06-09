using System.Collections.Generic;

namespace QuickFrame.Security.Data.Models {

	/// <summary>
	/// Represents a user that is assigned specific permissions within a site.
	/// </summary>
	public class SiteUser : UserBase {
		public virtual ICollection<SiteRole> Roles { get; set; }
	}
}