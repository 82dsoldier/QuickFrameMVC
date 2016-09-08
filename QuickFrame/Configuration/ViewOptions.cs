using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

/// <include file='Doc/Documentation.xml' path='QuickFrame/documentation[@name="Configuration"]'/> 
namespace QuickFrame.Configuration {

	/// <include file='Doc/Documentation.xml' path='QuickFrame/Configuration/documentation[@name="ViewOptions"]'/> 
	public class ViewOptions {

		/// <include file='Doc/Documentation.xml' path='QuickFrame/Configuration/ViewOptions/documentation[@name="ViewOptions"]'/> 
		public ViewOptions() {
			PerPageList = new List<SelectListItem>();
		}

		/// <include file='Doc/Documentation.xml' path='QuickFrame/Configuration/ViewOptions/documentation[@name="PerPageList"]'/> 
		public List<SelectListItem> PerPageList { get; set; }

		/// <include file='Doc/Documentation.xml' path='QuickFrame/Configuration/ViewOptions/documentation[@name="PerPageDefault"]'/> 
		public string PerPageDefault { get; set; }
	}
}
