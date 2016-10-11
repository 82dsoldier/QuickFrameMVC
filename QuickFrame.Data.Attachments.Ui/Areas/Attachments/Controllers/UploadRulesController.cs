using Microsoft.AspNetCore.Mvc;
using QuickFrame.Data.Attachments.Dtos;
using QuickFrame.Data.Attachments.Interfaces;
using QuickFrame.Data.Attachments.Models;
using QuickFrame.Mvc;
using QuickFrame.Mvc.Controllers;
using QuickFrame.Security;
using System.Data.SqlClient;

namespace QuickFrame.Data.Attachments.Ui.Areas.Attachments.Controllers {

	[Area("Attachments")]
	public class UploadRulesController : QfController<UploadRule, UploadRuleIndexDto, UploadRuleEditDto> {

		[HttpGet]
		public IActionResult Activate(int id) {
			var model = (_dataService as IUploadRulesDataService).Get<UploadRuleEditDto>(id);
			model.Priority = (_dataService as IUploadRulesDataService).GetLastPriority();
			return View(model);
		}

		[HttpPost]
		public IActionResult Activate(UploadRuleEditDto model) {
			ViewData["CloseOnSubmit"] = true;
			if(ModelState.IsValid) {
				model.IsActive = true;
				_dataService.Save(model);
			}

			return View(model);
		}

		[HttpGet]
		public IActionResult Deactivate(int id) {
			var model = (_dataService as IUploadRulesDataService).Get(id);
			model.IsActive = false;
			model.Priority = null;
			_dataService.Save(model);
			return new ObjectResult(true);
		}

		protected override IActionResult IndexCore<TResult>(string searchTerm = "", int page = 1, int itemsPerPage = 25, string sortColumn = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false) {
			return base.IndexCore<TResult>(searchTerm, page, itemsPerPage, "Priority", sortOrder, includeDeleted);
		}

		public UploadRulesController(IUploadRulesDataService dataService, QuickFrameSecurityManager securityManager) : base(dataService, securityManager) {
		}
	}
}