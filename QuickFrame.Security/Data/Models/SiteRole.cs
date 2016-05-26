using QuickFrame.Data.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuickFrame.Security.Data.Models {

	/// <summary>
	///     Identifies a security group within an application.
	/// </summary>
	public class SiteRole : DataModel {

		/// <summary>
		///     Gets or sets the name.
		/// </summary>
		/// <value>
		///     The name.
		/// </value>
		[Required]
		[StringLength(256)]
		public string Name { get; set; }

		/// <summary>
		///     Gets or sets the description.
		/// </summary>
		/// <value>
		///     The description.
		/// </value>
		[StringLength(2048)]
		public string Description { get; set; }

		/// <summary>
		///     A navigation property used to display all users assigned to this role.
		/// </summary>
		/// <value>
		///     The list of users assigned to this role.
		/// </value>
		public virtual ICollection<SiteUser> Users { get; set; }

		public virtual ICollection<SiteGroup> Groups { get; set; }
	}
}