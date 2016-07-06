using QuickFrame.Data;
using QuickFrame.Mapping;
using QuickFrame.Security.AccountControl.Data.Models;

namespace QuickFrame.Security.AccountControl.Data.Dtos {

	[ExpressMap]
	public class SiteGroupIndexDto : DataTransferObject<SiteGroup, SiteGroupIndexDto> {
		public string GroupId { get; set; }
		public string DisplayName { get; set; }
	}
}