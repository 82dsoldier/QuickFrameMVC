using QuickFrame.Data.Models;
using System;
using System.Collections.Generic;

namespace QuickFrame.Data.Attachments.Models {
	// Attachments

	public class Attachment : DataModelGuid {
		public byte[] Data { get; set; } // Data
		public string FileName { get; set; } // Name (length: 256)
		public string DocumentName { get; set; }
		public string DocumentId { get; set; } // DocumentNumber (length: 128)
		public string Description { get; set; } // Description (length: 2048)
		public DateTime UploadDate { get; set; }
		public Guid? ParentId { get; set; } // ParentId
		public Guid? PreviousId { get; set; } // PreviousId

		public virtual ICollection<Attachment> Children { get; set; } // Attachments.FK__Attachmen__Paren__46E78A0C

		public virtual Attachment Parent { get; set; } // FK__Attachmen__Paren__46E78A0C
	}
}

// </auto-generated>