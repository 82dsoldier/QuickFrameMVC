using QuickFrame.Attachments.Data.Dtos;
using QuickFrame.Attachments.Data.Models;
using QuickFrame.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuickFrame.Data.Interfaces;
using QuickFrame.Attachments.Data.Interfaces;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Http;
using System.IO;
using Microsoft.Net.Http.Headers;

namespace QuickFrame.Attachments.Data.Controllers
{
	public class AttachmentsController : ControllerGuid<Attachment, AttachmentDto, AttachmentDto> {
		public IActionResult CreateEx(bool closeOnSubmit = false) {
			TempData["CloseOnSubmit"] = closeOnSubmit;
			return View();
		}
		[HttpPost]
		public IActionResult CreateEx(IFormFile file) {
			var attachment = new Attachment();
			using (MemoryStream ms = new MemoryStream()) {
				using (var reader = file.OpenReadStream()) {
					reader.CopyTo(ms);
				}
				attachment.Data = ms.ToArray();
			}
			var parsedContentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
			attachment.Name = parsedContentDisposition.FileName;
			_dataService.Save(attachment);
			return View();
		}
		public AttachmentsController(IAttachmentsDataService dataService) 
			: base(dataService) {
		}
	}
}
