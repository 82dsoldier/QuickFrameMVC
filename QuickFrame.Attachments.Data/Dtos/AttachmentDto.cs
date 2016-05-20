using QuickFrame.Attachments.Data.Models;
using QuickFrame.Data;
using QuickFrame.Mapping;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Attachments.Data.Dtos
{
	[ExpressMap]
	public class AttachmentDto : DataTransferObjectGuid<Attachment, AttachmentDto>
    {
		public MemoryStream Data { get; set; }
		public string Name { get; set; }
		public string Comment { get; set; }
    }
}
