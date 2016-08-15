using QuickFrame.Data.Attachments.Models;
using QuickFrame.Mapping;
using System.Collections.Generic;

namespace QuickFrame.Data.Attachments.Dtos {

	[ExpressMap]
	public class AttachmentIndexDto : DataTransferObjectGuid<Attachment, AttachmentIndexDto> {
		public string Name { get; set; } // Name (length: 256)
		public string DocumentNumber { get; set; } // DocumentNumber (length: 128)
		public string Description { get; set; } // Description (length: 2048)
		public System.Guid? ParentId { get; set; } // ParentId
		public System.Guid? PreviousId { get; set; } // PreviousId

		// Reverse navigation
		public virtual List<AttachmentIndexDto> Children { get; set; } // Attachments.FK__Attachmen__Paren__46E78A0C
	}
}

// </auto-generated>