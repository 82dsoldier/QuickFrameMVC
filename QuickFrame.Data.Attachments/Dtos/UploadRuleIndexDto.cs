using ExpressMapper;
using QuickFrame.Data.Attachments.Models;
using QuickFrame.Data.Dtos;
using System;
using System.ComponentModel.DataAnnotations;

namespace QuickFrame.Data.Attachments.Dtos {

	public class UploadRuleIndexDto : DataTransferObject<UploadRule, UploadRuleIndexDto> {
		public int? Priority { get; set; }
		public string Name { get; set; } // Name (length: 64)
		public string Description { get; set; } // Description (length: 2048)

		[Display(Name = "Mime Type")]
		public string MimeTypeName { get; set; }

		[Display(Name = "File Extension")]
		public string FileExtensionName { get; set; }

		[Display(Name = "File Header Pattern")]
		public string FileHeaderPatternName { get; set; }

		[Display(Name = "Is Active")]
		public bool IsActive { get; set; }

		public override void Register() {
			Mapper.Register<UploadRule, UploadRuleIndexDto>()
				.Function(dest => dest.MimeTypeName, src => {
					return src.MimeType != null ? $"{src.MimeType.Name} ({src.MimeType.MimeTypeIdentifier})" : String.Empty;
				})
				.Function(dest => dest.FileExtensionName, src => {
					return src.FileExtension != null ? $"{src.FileExtension.Name} ({src.FileExtension.Extension})" : String.Empty;
				})
				.Function(dest => dest.FileHeaderPatternName, src => {
					return src.FileHeaderPattern != null ? src.FileHeaderPattern.Name : String.Empty;
				});
		}
	}
}