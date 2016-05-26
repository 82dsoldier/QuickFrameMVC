using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace QuickFrame.Configuration {

	/// <summary>
	/// The options from the appsettings.json file that provide paging information to the controllers
	/// </summary>
	public class ViewOptions {

		/// <summary>
		/// Initializes a new instance of the <see cref="ViewOptions"/> class.
		/// </summary>
		public ViewOptions() {
			PerPageList = new List<SelectListItem>();
		}

		/// <summary>
		/// Gets or sets the per page list.
		/// </summary>
		/// <value>
		/// The per page list.
		/// </value>
		public List<SelectListItem> PerPageList { get; set; }

		/// <summary>
		/// Gets or sets the per page default.
		/// </summary>
		/// <value>
		/// The per page default.
		/// </value>
		public string PerPageDefault { get; set; }
	}
}