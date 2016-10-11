using Microsoft.AspNetCore.Mvc;
using QuickFrame.Data.Attachments.Dtos;
using QuickFrame.Data.Attachments.Interfaces;
using QuickFrame.Data.Attachments.Models;
using QuickFrame.Mvc;
using QuickFrame.Mvc.Controllers;
using QuickFrame.Security;

namespace QuickFrame.Data.Attachments.Areas.Attachments.Controllers {

	[Area("Attachments")]
	public class MimeTypesController : QfController<MimeType, MimeTypeDto, MimeTypeDto> {

		public MimeTypesController(IMimeTypesDataService dataService, QuickFrameSecurityManager securityManager) : base(dataService, securityManager) {
		}
	}
}