using ExpressMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuickFrame.Security.AccountControl.Models;
using QuickFrame.Security.Data;
using QuickFrame.Security.Data.Dtos;
using QuickFrame.Security.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace QuickFrame.Security.Areas.TransactionLog.Controllers {

	[Area("TransactionLog")]
	public class TrackingController : Controller {
		private TrackingContext _dbContext;
		private UserManager<SiteUser> _userManager;

		public TrackingController(TrackingContext dbContext, UserManager<SiteUser> userManager) {
			_dbContext = dbContext;
			_userManager = userManager;
		}

		public IActionResult Index() {
			var recordList = new List<AuditLogIndexDto>();
			foreach(var record in _dbContext.AuditLogs)
				recordList.Add(Mapper.Map<AuditLog, AuditLogIndexDto>(record));
			return View(recordList);
		}

		public IActionResult Details(int id) {
			return GetDetailsView("Details", id);
		}

		public IActionResult LogDetails(int id) {
			return GetDetailsView("LogDetails", id);
		}

		private IActionResult GetDetailsView(string viewName, int id) {
			var model = Mapper.Map<AuditLog, AuditLogDetailDto>(_dbContext.AuditLogs.AsNoTracking().First(log => log.Id == id));
			var userTask = _userManager.FindByIdAsync(model.UserId);
			userTask.Wait();
			model.UserId = userTask.Result.DisplayName;
			return View(viewName, model);
		}
	}
}