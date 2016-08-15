using QuickFrame.Data.Attachments.Models;
using System;

namespace QuickFrame.Data.Attachments.Dtos {

	public class AttachmentCreateRevisionDto : DataTransferObjectGuid<Attachment, AttachmentCreateRevisionDto> {
		public Guid ParentId { get; set; }
		public Guid PreviousId { get; set; }
		public bool IsRevision { get; set; }
	}
}