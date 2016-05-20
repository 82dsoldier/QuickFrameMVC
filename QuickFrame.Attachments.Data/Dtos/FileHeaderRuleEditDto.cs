using ExpressMapper;
using QuickFrame.Attachments.Data.Models;
using QuickFrame.Data;
using QuickFrame.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFrame.Attachments.Data.Dtos
{
	[ExpressMap]
	public class FileHeaderRuleEditDto : DataTransferObject<FileHeaderRule, FileHeaderRuleEditDto>
    {
		[Display(Name="Rule Name")]
		[StringLength(64)]
		public string Name { get; set; }
		[Display(Name="Rule Description")]
		[StringLength(1024)]
		public string Description { get; set; }
		[Display(Name="File Header")]
		public string FileHeader { get; set; }
		public int Offset { get; set; }
		[Display(Name="Start Location")]
		public int Location { get; set; }
		[Display(Name="Mime Type")]
		public int? MimeTypeId { get; set; }
		[Display(Name="File Extension")]
		[StringLength(32)]
		public string FileExtension { get; set; }
		[Display(Name="Must Match")]
		public bool MustMatchExtension { get; set; }
		[Display(Name="Must Match")]
		public bool MustMatchMimeType { get; set; }
		[Display(Name="Is Allowed")]
		public bool IsAllowed { get; set; }

		public override void Register() {
			Mapper.Register<FileHeaderRule, FileHeaderRuleEditDto>()
				.Function(dest => dest.Location, src => {
					return src.Location ? 1 : 0;
				})
				.Function(dest => dest.FileHeader, src => {
					var retVal = BitConverter.ToString(src.FileHeader).Replace("-", ", 0x");
					return $"0x{retVal}";
				});
			Mapper.Register<FileHeaderRuleEditDto, FileHeaderRule>()
				.Function(dest => dest.Location, src => {
					return src.Location == 0 ? false : true;
				})
				.Function(dest => dest.FileHeader, src => {
					var splitString = src.FileHeader.Split(',');
					byte[] buffer = new byte[splitString.Length];
					int pos = 0;
					foreach (var s in splitString) {
						buffer[pos++] = Convert.ToByte(s.Trim().Replace("\n", "").Replace("\r", ""), 16);
					}
					return buffer;
				});
		}
	}
}
