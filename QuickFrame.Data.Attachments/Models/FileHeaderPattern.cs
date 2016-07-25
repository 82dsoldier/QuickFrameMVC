
using QuickFrame.Data.Models;
using System.Collections.Generic;

namespace QuickFrame.Data.Attachments.Models
{

    // FileHeaderPatterns
    
    public class FileHeaderPattern : DataModel
    {
		public string Name { get; set; } // Name (length: 64)
		public string Description { get; set; } // Description (length: 2048)
		public bool Location { get; set; } // Location
		public int Offset { get; set; } // Offset
		public byte[] FileHeader { get; set; } // FileHeader (length: 2048)

		// Reverse navigation
		public virtual ICollection<UploadRule> UploadRules { get; set; } // UploadRules.FK__UploadRul__FileH__4222D4EF

	}

}
// </auto-generated>
