using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace QuickFrame.Mvc.Configuration {

	public class ViewOptions {

		public ViewOptions() {
			PerPageList = new List<SelectListItem>();
		}

		public List<SelectListItem> PerPageList { get; set; }

		public string PerPageDefault { get; set; }
	}
}