using QuickFrame.Security.AccountControl.Dtos;
using QuickFrame.Security.AccountControl.Models;
using System.Collections.Generic;

namespace QuickFrame.Security.Areas.Security.Models {

	public class RoleRuleIndexModel {
		public int RoleId { get; set; }
		public List<SiteRoleIndexDto> RoleList { get; set; } = new List<SiteRoleIndexDto>();
	}
}
