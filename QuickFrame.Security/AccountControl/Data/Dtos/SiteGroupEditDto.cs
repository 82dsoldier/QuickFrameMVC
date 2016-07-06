using QuickFrame.Data;
using QuickFrame.Mapping;
using QuickFrame.Security.AccountControl.Data.Models;

namespace QuickFrame.Security.AccountControl.Data.Dtos {

	[ExpressMap]
	public class SiteGroupEditDto : DataTransferObject<SiteGroup, SiteGroupEditDto> {
		public string GroupId { get; set; }
		public string DisplayName { get; set; }
		public int RoleId { get; set; }
	}
}