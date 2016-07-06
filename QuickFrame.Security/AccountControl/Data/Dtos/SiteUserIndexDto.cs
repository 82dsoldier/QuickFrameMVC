using QuickFrame.Data;
using QuickFrame.Mapping;
using QuickFrame.Security.AccountControl.Data.Models;

namespace QuickFrame.Security.AccountControl.Data.Dtos {

	[ExpressMap]
	public class SiteUserIndexDto : DataTransferObject<SiteUser, SiteUserIndexDto> {
		public string DisplayName { get; set; }
	}
}