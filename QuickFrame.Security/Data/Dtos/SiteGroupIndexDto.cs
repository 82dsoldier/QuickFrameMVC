using QuickFrame.Data;
using QuickFrame.Mapping;
using QuickFrame.Security.Data.Models;

namespace QuickFrame.Security.Data.Dtos {

	[ExpressMap]
	public class SiteGroupIndexDto : DataTransferObject<SiteGroup, SiteGroupIndexDto> {
		public string GroupId { get; set; }
		public string DisplayName { get; set; }
	}
}