using QuickFrame.Data.Models;
using System.Collections.Generic;

namespace QuickFrame.Data.Attachments.Models {
	// FileExtensions

	public class FileExtension : NamedDataModel {
		public string Description { get; set; } // Description (length: 2048)
		public string Extension { get; set; } // Extension (length: 32)

		// Reverse navigation
		public virtual ICollection<UploadRule> UploadRules { get; set; } // UploadRules.FK__UploadRul__FileE__412EB0B6
	}
}

// </auto-generated>