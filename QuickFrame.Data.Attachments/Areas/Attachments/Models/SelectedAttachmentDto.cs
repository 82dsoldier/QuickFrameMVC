using System;

namespace QuickFrame.Data.Attachments.Areas.Attachments.Models {

	public class SelectedAttachmentDto {
		public string Area { get; set; }
		public string Controller { get; set; }
		public string Action { get; set; }
		public Guid AttachmentId { get; set; }
	}
}