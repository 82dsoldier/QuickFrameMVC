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

namespace QuickFrame.Attachments.Data.Controllers
{
	public class MimeTypesController : ControllerBase<MimeType, MimeTypeIndexDto, MimeTypeIndexDto> {

		protected override IActionResult GetBase() => Authorize(User, () => new ObjectResult(_dataService.GetList<MimeTypeGetDto>()));
		public MimeTypesController(IMimeTypesDataService dataService) : base(dataService) {
		}
	}
}
