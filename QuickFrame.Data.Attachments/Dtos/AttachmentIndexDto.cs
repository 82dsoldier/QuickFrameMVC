using QuickFrame.Data.Attachments.Models;
using QuickFrame.Data.Dtos;
using System;
using System.Collections.Generic;

namespace QuickFrame.Data.Attachments.Dtos {

	public class AttachmentIndexDto : DataTransferObjectGuid<Attachment, AttachmentIndexDto> {
		public string Name { get; set; } // Name (length: 256)
		public string DocumentNumber { get; set; } // DocumentNumber (length: 128)
		public string Description { get; set; } // Description (length: 2048)
		public Guid? ParentId { get; set; } // ParentId
		public Guid? PreviousId { get; set; } // PreviousId

		public virtual List<AttachmentIndexDto> Children { get; set; } // Attachments.FK__Attachmen__Paren__46E78A0C
	}
}

// </auto-generated>