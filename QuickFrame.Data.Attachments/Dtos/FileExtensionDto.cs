using QuickFrame.Data;
using QuickFrame.Data.Attachments.Interfaces;
using QuickFrame.Data.Attachments.Models;
using QuickFrame.Di;
using QuickFrame.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System.IO;

namespace QuickFrame.Data.Attachments.Dtos
{
	[ExpressMap]
	public class FileExtensionDto : DataTransferObject<FileExtension, FileExtensionDto>, IValidatableObject, IUploadRuleDto
    {
		[StringLength(64)]
		public string Name { get; set; }
		[StringLength(2048)]
		public string Description { get; set; }
		[StringLength(32)]
		public string Extension { get; set; }
		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
			using(var service = ComponentContainer.Component<IFileExtensionsDataService>()) {
				if(service.Component.FilExtensionExists(Id, Name))
					yield return new ValidationResult($"THe extension {Name} already exists in the database.");
			}
		}

		public bool IsMatch(IFormFile file) {
			var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
			var fileExtension = Path.GetExtension(fileName);
			return fileExtension.Equals(Extension, StringComparison.CurrentCultureIgnoreCase);
		}
	}
}
