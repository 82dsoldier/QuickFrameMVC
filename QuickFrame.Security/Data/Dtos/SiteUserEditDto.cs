using QuickFrame.Data;
using QuickFrame.Mapping;
using QuickFrame.Security.Data.Models;

namespace QuickFrame.Security.Data.Dtos {

	[ExpressMap]
	public class SiteUserEditDto : DataTransferObject<SiteUser, SiteUserEditDto> {
		public string UserId { get; set; }
		public string DisplayName { get; set; }
		public int RoleId { get; set; }
	}
}