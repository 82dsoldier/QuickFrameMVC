using ExpressMapper;
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
	public class FileHeaderRuleIndexDto : DataTransferObject<FileHeaderRule, FileHeaderRuleIndexDto>
    {
		[StringLength(64)]
		public string Name { get; set; }
		[StringLength(1024)]
		public string Description { get; set; }
		public string MimeType { get; set; }
		public string FileExtension { get; set; }
		public bool MustMatchExtension { get; set; }
		public bool MustMatchMimeType { get; set; }
		public bool IsAllowed { get; set; }

		public override void Register() {
			Mapper.Register<FileHeaderRule, FileHeaderRuleIndexDto>()
				.Member(dest => dest.MimeType, src => $"{src.MimeType.Name} ({src.MimeType.FileExtension})");
		}
	}
}
