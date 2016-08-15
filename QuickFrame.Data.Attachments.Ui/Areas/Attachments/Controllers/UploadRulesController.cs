using Microsoft.AspNetCore.Mvc;
using QuickFrame.Data.Attachments.Dtos;
using QuickFrame.Data.Attachments.Interfaces;
using QuickFrame.Data.Attachments.Models;
using QuickFrame.Mvc;
using System.Data.SqlClient;

namespace QuickFrame.Data.Attachments.Ui.Areas.Attachments.Controllers {

	[Area("Attachments")]
	public class UploadRulesController : ControllerCore<UploadRule, UploadRuleIndexDto, UploadRuleEditDto> {

		[HttpGet]
		public IActionResult Activate(int id) {
			var model = _dataService.Get<UploadRuleEditDto>(id);
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
			var model = _dataService.Get(id);
			model.IsActive = false;
			model.Priority = null;
			_dataService.Save(model);
			return new ObjectResult(true);
		}

		protected override IActionResult IndexBase<TResult>(int page = 1, int itemsPerPage = 25, string sortColumn = "Name", SortOrder sortOrder = SortOrder.Ascending) {
			return base.IndexBase<TResult>(page, itemsPerPage, "Priority", sortOrder);
		}

		public UploadRulesController(IUploadRulesDataService dataService) : base(dataService) {
		}
	}
}