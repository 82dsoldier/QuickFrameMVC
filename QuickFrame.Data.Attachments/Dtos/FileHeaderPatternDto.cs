using ExpressMapper;
using QuickFrame.Data.Attachments.Interfaces;
using QuickFrame.Data.Attachments.Models;
using QuickFrame.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace QuickFrame.Data.Attachments.Dtos
{
	[ExpressMap]
	public class FileHeaderPatternDto : DataTransferObject<FileHeaderPattern, FileHeaderPatternDto>, IUploadRuleDto {
		private static Regex stringMap = new Regex(@"([A-Fa-f0-9][A-Fa-f0-9]?)[,\s]?", RegexOptions.Compiled);

		[StringLength(64)]
		[Required]
		public string Name { get; set; } // Name (length: 64)
		[StringLength(1024)]
		public string Description { get; set; } // Description (length: 1024)
		public bool LocationBeginning { get; set; } = true;// Location
		public bool LocationEnd { get; set; } = false;
		public int Offset { get; set; } = 0;// Offset
		[RegularExpression(@"(([A-Fa-f0-9][A-Fa-f0-9]?)[,\s]?)+", ErrorMessage = "Offset value must be a series of valid hexidecimal numbers seperated by a space or comma.")]
		public string FileHeader { get; set; } // FileHeader (length: 2048)

		public override void Register() {
			Mapper.Register<FileHeaderPattern, FileHeaderPatternDto>()
				.Member(dest => dest.LocationBeginning, src => src.Location)
				.Member(dest => dest.LocationEnd, src => !src.Location)
				.Function(dest => dest.FileHeader, src => {
					var buffer = BitConverter.ToString(src.FileHeader);
					return buffer.Replace('-', ',');
				});

			Mapper.Register<FileHeaderPatternDto, FileHeaderPattern>()
				.Member(dest => dest.Location, src => src.LocationBeginning)
				.Function(dest => dest.FileHeader, src => {
					MatchCollection collection = stringMap.Matches(src.FileHeader);
					byte[] buffer = new byte[collection.Count];
					int i = 0;
					foreach(Match m in collection)
						if(m.Groups.Count > 1)
							buffer[i++] = Convert.ToByte(m.Groups[1].Value, 16);
					return buffer;
				});
		}

		public bool IsMatch(IFormFile file) {
			MatchCollection collection = stringMap.Matches(FileHeader);
			byte[] buffer = new byte[collection.Count];
			int i = 0;
			foreach(Match m in collection)
				if(m.Groups.Count > 1)
					buffer[i++] = Convert.ToByte(m.Groups[1].Value, 16);
			using(MemoryStream ms = new MemoryStream()) {
				file.CopyTo(ms);
				var fileBytes = ms.ToArray();
				var selectedBytes = new byte[1];
				if(LocationBeginning)
					selectedBytes = fileBytes.Skip(Offset).Take(buffer.Length).ToArray();
				return selectedBytes.Equals(buffer);
			}
		}
	}
}
