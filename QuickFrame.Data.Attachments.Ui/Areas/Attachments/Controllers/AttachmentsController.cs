using Microsoft.AspNetCore.Mvc;
using QuickFrame.Data.Attachments.Dtos;
using QuickFrame.Data.Attachments.Interfaces;
using QuickFrame.Data.Attachments.Models;
using QuickFrame.Mvc;
using QuickFrame.Mvc.Controllers;
using QuickFrame.Security;
using System;

namespace QuickFrame.Data.Attachments.Ui.Areas.Attachments.Controllers {

	[Area("Attachments")]
	public class AttachmentsController : QfControllerGuid<Attachment, AttachmentIndexDto, AttachmentCreateDto> {

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

		protected override IActionResult CreateCore<TModel>(TModel model) {
			var id = (_dataService as IAttachmentsDataService).CreateAttachment(model);
			var parentId = (_dataService as IAttachmentsDataService).FindParent(id);
			if(parentId == null)
				return View("Create");
			return RedirectToAction("CreateRevision", new { id = id, parentId = parentId });
		}

		public AttachmentsController(IAttachmentsDataService dataService, QuickFrameSecurityManager securityManager) : base(dataService, securityManager) {
			CreatePage = "Create";
		}
	}
}