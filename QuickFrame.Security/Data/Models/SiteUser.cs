using QuickFrame.Data.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuickFrame.Security.Data.Models {

	/// <summary>
	/// Represents a user that is assigned specific permissions within a site.
	/// </summary>
	public class SiteUser : DataModel {

		/// <summary>
		/// Gets or sets the name of the user. Should be set to the active directory name.
		/// </summary>
		/// <value>The name of the user.</value>
		[Required]
		[StringLength(128)]
		public string UserId { get; set; }

		[StringLength(256)]
		public string DisplayName { get; set; }

		[StringLength(64)]
		public string FirstName { get; set; }

		public string LastName { get; set; }
		public string Email { get; set; }

		public virtual ICollection<SiteRole> Roles { get; set; }
	}
}