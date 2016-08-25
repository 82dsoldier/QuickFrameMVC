using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Data.Attachments.Areas.Attachments.Models
{
    public class CreateStartModel
    {
		public AttachmentCreationType AttachmentCreationType { get; set; }
    }

	public enum AttachmentCreationType {
		UploadNew,
		UploadRevision,
		AttachNew,
		AttachRevision
	}
}
