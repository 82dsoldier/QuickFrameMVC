using QuickFrame.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Attachments.Data.Models
{
    public class MimeType : DataModel
    {
		[StringLength(256)]
		public string Name { get; set; }
		[StringLength(256)]
		public string DataType { get; set; }
		[StringLength(24)]
		public string FileExtension { get; set; }

		public virtual ICollection<MimeTypeRule> MimeTypeValidations { get; set; }
		public virtual ICollection<Attachment> Attachments { get; set; }
		public virtual ICollection<FileHeaderRule> FileHeaderRules { get; set; }
    }
}
