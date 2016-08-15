using Microsoft.AspNetCore.Http;
using QuickFrame.Data.Attachments.Interfaces;
using QuickFrame.Data.Attachments.Models;

namespace QuickFrame.Data.Attachments.Dtos {

	public class UploadRuleDto : DataTransferObject<UploadRule, UploadRuleDto> {
		public string Name { get; set; } // Name (length: 64)
		public string Description { get; set; } // Description (length: 2048)
		public int? Priority { get; set; } // Priority
		public bool IsActive { get; set; } // IsActive
		public bool IsAllow { get; set; } // IsAllow
		public int? FileExtensionId { get; set; } // FileExtensionId
		public int? FileHeaderPatternId { get; set; } // FileHeaderPatternId
		public int? MimeTypeId { get; set; } // MimeTypeId

		// Foreign keys
		public FileExtensionDto FileExtension { get; set; } // FK__UploadRul__FileE__412EB0B6

		public FileHeaderPatternDto FileHeaderPattern { get; set; } // FK__UploadRul__FileH__4222D4EF
		public MimeTypeDto MimeType { get; set; } // FK__UploadRul__MimeT__4316F928

		public bool CanFileUpload(IFormFile file) {
			if((FileExtension as IUploadRuleDto).IsMatch(file))
				if(!IsAllow)
					return false;
			if((FileHeaderPattern as IUploadRuleDto).IsMatch(file))
				if(!IsAllow)
					return false;
			if((MimeType as IUploadRuleDto).IsMatch(file))
				if(!IsAllow)
					return false;

			return true;
		}
	}
}