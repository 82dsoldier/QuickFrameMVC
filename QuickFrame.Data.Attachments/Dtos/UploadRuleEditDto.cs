using QuickFrame.Data.Attachments.Models;
using QuickFrame.Mapping;
using System.ComponentModel.DataAnnotations;

namespace QuickFrame.Data.Attachments.Dtos {

	[ExpressMap]
	public class UploadRuleEditDto : DataTransferObject<UploadRule, UploadRuleEditDto> {

		[StringLength(64)]
		public string Name { get; set; } // Name (length: 64)

		public int Priority { get; set; }

		[StringLength(2048)]
		public string Description { get; set; } // Description (length: 2048)

		[Display(Name = "Mime Type")]
		public int MimeTypeId { get; set; }

		[Display(Name = "File Extension")]
		public int FileExtensionId { get; set; }

		[Display(Name = "File Header Pattern")]
		public int FileHeaderPatternId { get; set; }

		[Display(Name = "Is Active")]
		public bool IsActive { get; set; }

		[Display(Name = "Is Allow Rule")]
		public bool IsAllow { get; set; }
	}
}