using QuickFrame.Data.Models;
using System.Collections.Generic;

namespace QuickFrame.Data.Attachments.Models {
	// MimeTypes

	public class MimeType : NamedDataModel {
		public string Description { get; set; } // Description (length: 2048)
		public string MimeTypeIdentifier { get; set; } // MimeTypeIdentifier (length: 128)

		// Reverse navigation
		public virtual ICollection<UploadRule> UploadRules { get; set; } // UploadRules.FK__UploadRul__MimeT__4316F928
	}
}

// </auto-generated>