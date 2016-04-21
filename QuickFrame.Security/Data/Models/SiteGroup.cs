using QuickFrame.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuickFrame.Security.Data.Models {

	public class SiteGroup : DataModel {

		[Required]
		[StringLength(128)]
		public string GroupId { get; set; }

		[StringLength(256)]
		public string DisplayName { get; set; }

		public virtual ICollection<SiteRole> Roles { get; set; }
	}
}