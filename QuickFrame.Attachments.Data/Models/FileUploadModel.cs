using Microsoft.AspNet.Http;
using Microsoft.Net.Http.Headers;
using QuickFrame.Attachments.Data.Interfaces;
using QuickFrame.Di;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Attachments.Data.Models
{
    public class FileUploadModel : IValidatableObject
    {
		public IFormFile File { get; set; }
		public string Comments { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
			using(var container = ComponentContainer.Component<IMimeTypeRulesDataService>()) {
				var parsedContentDisposition = ContentDispositionHeaderValue.Parse(File.ContentDisposition);
				if (!container.Component.IsFileAllowed(parsedContentDisposition.FileName))
					yield return new ValidationResult("This file type is not allowed to be uploaded.");
				yield break;
			}
		}
	}
}
