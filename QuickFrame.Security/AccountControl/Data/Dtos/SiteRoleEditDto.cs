using QuickFrame.Data;
using QuickFrame.Mapping;
using QuickFrame.Security.AccountControl.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace QuickFrame.Security.AccountControl.Data.Dtos {

	[ExpressMap]
	public class SiteRoleEditDto : DataTransferObject<SiteRole, SiteRoleEditDto> {

		[StringLength(2048)]
		public string Description { get; set; }
	}
}