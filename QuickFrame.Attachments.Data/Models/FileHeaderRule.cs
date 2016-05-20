using QuickFrame.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Attachments.Data.Models
{
    public class FileHeaderRule : DataModel
    {
		[StringLength(64)]
		public string Name { get; set; }
		[StringLength(1024)]
		public string Description { get; set; }
		public byte[] FileHeader { get; set; }
		public int Offset { get; set; }
		public bool Location { get; set; }
		public int? MimeTypeId { get; set; }
		[StringLength(32)]
		public string FileExtension { get; set; }
		public bool MustMatchExtension { get; set; }
		public bool MustMatchMimeType { get; set; }
		public bool IsAllowed { get; set; }

		public virtual MimeType MimeType { get; set; }
    }
}
