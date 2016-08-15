using Microsoft.AspNetCore.Http;
using QuickFrame.Data.Attachments.Interfaces;
using QuickFrame.Data.Attachments.Models;
using QuickFrame.Mapping;
using System;
using System.ComponentModel.DataAnnotations;

namespace QuickFrame.Data.Attachments.Dtos {
	// MimeTypes

	[ExpressMap]
	public class MimeTypeDto : DataTransferObject<MimeType, MimeTypeDto>, IUploadRuleDto {

		[StringLength(256)]
		[Required]
		public string Name { get; set; } // Name (length: 256)

		[StringLength(2048)]
		public string Description { get; set; }

		[StringLength(256)]
		[Required]
		[Display(Name = "Mime Type")]
		public string MimeTypeIdentifier { get; set; } // DataType (length: 256)

		public bool IsMatch(IFormFile file) {
			return file.ContentType.Equals(MimeTypeIdentifier, StringComparison.CurrentCultureIgnoreCase);
		}
	}
}

// </auto-generated>