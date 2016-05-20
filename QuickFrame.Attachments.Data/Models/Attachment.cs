using QuickFrame.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Attachments.Data.Models
{
    public class Attachment : DataModelGuid
    {
		public byte[] Data { get; set; }
		[StringLength(256)]
		public string Name { get; set; }
		[StringLength(2048)]
		public string Comment { get; set; }
		public int MimeTypeId { get; set; }
		public virtual MimeType MimeType { get; set; }
    }
}
