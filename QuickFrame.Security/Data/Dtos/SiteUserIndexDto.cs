using QuickFrame.Data;
using QuickFrame.Mapping;
using QuickFrame.Security.Data.Models;

namespace QuickFrame.Security.Data.Dtos {

	[ExpressMap]
	public class SiteUserIndexDto : DataTransferObject<SiteUser, SiteUserIndexDto> {
		public string DisplayName { get; set; }
	}
}