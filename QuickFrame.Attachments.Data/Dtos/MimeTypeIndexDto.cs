using QuickFrame.Attachments.Data.Models;
using QuickFrame.Data;
using QuickFrame.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Attachments.Data.Dtos
{
	[ExpressMap]
	public class MimeTypeIndexDto : DataTransferObject<MimeType, MimeTypeIndexDto>
    {
		[StringLength(256)]
		public string Name { get; set; }
		[StringLength(256)]
		public string DataType { get; set; }
		[StringLength(24)]
		public string FileExtension { get; set; }
	}
}
