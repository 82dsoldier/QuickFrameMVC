
using ExpressMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using QuickFrame.Data.Attachments.Models;
using QuickFrame.Data.Models;
using QuickFrame.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace QuickFrame.Data.Attachments.Dtos
{
	[ExpressMap]
    public class AttachmentCreateDto : DataTransferObjectGuid<Attachment, AttachmentCreateDto>
    {
		public IFormFile Data { get; set; } // Data
		[Display(Name ="Document Id")]
		public string DocumentId { get; set; } // DocumentNumber (length: 128)
		public string Description { get; set; } // Description (length: 2048)
		public override void Register() {
			Mapper.Register<AttachmentCreateDto, Attachment>()
				.Function(dest => dest.FileName, src => {
					return ContentDispositionHeaderValue.Parse(Data.ContentDisposition).FileName;
				})
				.Member(dest => dest.UploadDate, src => DateTime.Now)
				.Function(dest => dest.Data, src => {
					using(MemoryStream ms = new MemoryStream()) {
						src.Data.CopyTo(ms);
						return ms.ToArray();
					}
				});
		}
	}

}
// </auto-generated>
