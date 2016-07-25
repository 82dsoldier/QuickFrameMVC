using QuickFrame.Data.Attachments.Models;
using QuickFrame.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuickFrame.Data.Interfaces;
using QuickFrame.Data.Attachments.Interfaces;
using QuickFrame.Data.Attachments.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace QuickFrame.Data.Attachments.Ui.Areas.Attachments.Controllers
{
	[Area("Attachments")]
	public class AttachmentsController : ControllerGuid<Attachment, AttachmentIndexDto, AttachmentCreateDto> {
		[HttpGet]
		public IActionResult CreateRevision(Guid id, Guid parentId) {
			return View(new AttachmentCreateRevisionDto {
				Id = id,
				ParentId = parentId
			});
		}

		[HttpPost]
		public IActionResult CreateRevision(AttachmentCreateRevisionDto model) {
			if(model.IsRevision) {
				(_dataService as IAttachmentsDataService).CreateRevision(model);
			}
			return View();
		}

		protected override IActionResult CreateBase<TReturn>(bool closeOnSubmit = false, string modelName = "CreateOrEdit") {
			return base.CreateBase<AttachmentCreateDto>(closeOnSubmit, "Create");
		}

		protected override IActionResult CreateBase<TModel>(TModel model, string modelName = "CreateOrEdit") {
			var id = (_dataService as IAttachmentsDataService).CreateAttachment(model);
			var parentId = (_dataService as IAttachmentsDataService).FindParent(id);
			if(parentId == null)
				return View("Create");
			return RedirectToAction("CreateRevision", new { id = id, parentId = parentId });
		}
		public AttachmentsController(IAttachmentsDataService dataService) : base(dataService) {
		}
	}
}