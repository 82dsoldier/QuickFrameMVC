using QuickFrame.Data.Dtos;
using QuickFrame.Security.AccountControl.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace QuickFrame.Security.AccountControl.Data.Dtos {

	public class SiteRoleIndexDto : GenericDataTransferObject<SiteRole, SiteRoleIndexDto> {
		public string Id { get; set; }

		[StringLength(128)]
		[Required]
		public string Name { get; set; }

		[StringLength(2048)]
		public string Description { get; set; }
	}
}