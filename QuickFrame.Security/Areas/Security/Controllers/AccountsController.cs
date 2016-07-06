using Microsoft.AspNetCore.Mvc;
using QuickFrame.Security.AccountControl.Data.Models;
using QuickFrame.Security.AccountControl.Interfaces;
using QuickFrame.Security.Areas.Security.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Security.Areas.Security.Controllers
{
	[Area("Security")]
    public class AccountsController : Controller
    {
		private IAccountDataService _accountEnumerator;
		
		[HttpGet]
		public IEnumerable<UserBase> GetUsers(string filter = "") {
			return _accountEnumerator.GetUsers<UserBase>(filter);
		}
		[HttpGet]
		public IActionResult UserList(string returnController, string returnAction, string filter = "") {
			return View(new UserListModel {
				ReturnAction = returnAction,
				ReturnController = returnController,
				Filter = filter
			});
		}

		[HttpPost]
		public IActionResult UserList(UserListModel model) {
			return RedirectToAction( model.ReturnAction, model.ReturnController,new { area = "", userId = model.UserId });
		}
		public AccountsController(IAccountDataService accountEnumerator) {
			_accountEnumerator = accountEnumerator;
		}
    }
}
