using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using QuickFrame.Data.Attachments.Interfaces;
using QuickFrame.Data.Attachments.Models;
using QuickFrame.Data.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace QuickFrame.Data.Attachments.Dtos {

	public class FileExtensionDto : NamedDataTransferObject<FileExtension, FileExtensionDto>, IUploadRuleDto {

		[StringLength(2048)]
		public string Description { get; set; }

		[StringLength(32)]
		public string Extension { get; set; }

		public bool IsMatch(IFormFile file) {
			var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
			var fileExtension = Path.GetExtension(fileName);
			return fileExtension.Equals(Extension, StringComparison.CurrentCultureIgnoreCase);
		}
	}
}