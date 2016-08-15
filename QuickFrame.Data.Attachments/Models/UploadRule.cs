using QuickFrame.Data.Models;

namespace QuickFrame.Data.Attachments.Models {
	// AllRules

	public class UploadRule : DataModel {
		public string Name { get; set; } // Name (length: 64)
		public string Description { get; set; } // Description (length: 2048)
		public int? Priority { get; set; } // Priority
		public bool IsActive { get; set; } // IsActive
		public bool IsAllow { get; set; } // IsAllow
		public int? FileExtensionId { get; set; } // FileExtensionId
		public int? FileHeaderPatternId { get; set; } // FileHeaderPatternId
		public int? MimeTypeId { get; set; } // MimeTypeId

		// Foreign keys
		public virtual FileExtension FileExtension { get; set; } // FK__UploadRul__FileE__412EB0B6

		public virtual FileHeaderPattern FileHeaderPattern { get; set; } // FK__UploadRul__FileH__4222D4EF
		public virtual MimeType MimeType { get; set; } // FK__UploadRul__MimeT__4316F928
	}
}

// </auto-generated>