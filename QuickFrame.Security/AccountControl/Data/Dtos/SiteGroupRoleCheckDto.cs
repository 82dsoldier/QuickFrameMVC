using QuickFrame.Data;
using QuickFrame.Mapping;
using QuickFrame.Security.AccountControl.Data.Models;
using System.Collections.Generic;

namespace QuickFrame.Security.AccountControl.Data.Dtos {

	[ExpressMap]
	public class SiteGroupRoleCheckDto : DataTransferObject<SiteGroup, SiteGroupRoleCheckDto> {
		public string GroupId { get; set; }
		public string DisplayName { get; set; }
		public List<SiteRoleIndexDto> Roles { get; set; }
	}
}