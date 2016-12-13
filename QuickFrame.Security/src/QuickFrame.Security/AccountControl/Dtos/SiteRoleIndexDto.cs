using QuickFrame.Data.Dtos;
using QuickFrame.Security.AccountControl.Models;
using System.ComponentModel.DataAnnotations;

namespace QuickFrame.Security.AccountControl.Dtos {

	public class SiteRoleIndexDto : DataTransferObjectCore<SiteRole, SiteRoleIndexDto> {
		public string Id { get; set; }

		[StringLength(128)]
		[Required]
		public string Name { get; set; }

		[StringLength(2048)]
		public string Description { get; set; }
		public string NormalizedName { get; set; }
	}
}
