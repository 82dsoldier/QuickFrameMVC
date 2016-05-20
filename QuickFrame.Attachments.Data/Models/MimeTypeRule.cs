using QuickFrame.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Attachments.Data.Models
{
    public class MimeTypeRule : DataModel
    {
		public int MimeTypeId { get; set; }
		public bool IncludeType { get; set; }

		public virtual MimeType MimeType { get; set; }
    }
}
