using ExpressMapper;
using QuickFrame.Attachments.Data.Models;
using QuickFrame.Data;
using QuickFrame.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Attachments.Data.Dtos
{
	[ExpressMap]
    public class MimeTypeGetDto : DataTransferObject<MimeType, MimeTypeGetDto>
    {
		public string Name { get; set; }

		public override void Register() {
			Mapper.Register<MimeType, MimeTypeGetDto>()
				.Member(dest => dest.Name, src => $"{src.Name} ({src.FileExtension})");
		}
	}
}
