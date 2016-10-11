using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace QuickFrame.Mvc.Models {

	public class PagingComponentModel {
		public int CurrentPage { get; set; }
		public int ItemsPerPage { get; set; }
		public int TotalPages { get; set; }
		public string SortColumn { get; set; }
		public SortOrder SortOrder { get; set; }
		public string Area { get; set; }
		public string Controller { get; set; }
		public string Action { get; set; }
		public List<SelectListItem> ItemsPerPageList { get; set; } = new List<SelectListItem>();
	}
}