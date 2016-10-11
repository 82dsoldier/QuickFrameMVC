using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QuickFrame.Security.AccountControl.Models;
using QuickFrame.Security.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuickFrame.Security.Areas.Security.Controllers {

	[Area("Security")]
	public class UsersController : Controller {
		private UserManager<SiteUser> _userManager;
		private NameListOptions _nameList;

		[HttpGet]
		public IEnumerable<IdentityUser> GetUsers(string filter = "") {
			var userList = new List<string>();
			if(!String.IsNullOrEmpty(filter)) {
				foreach(var user in filter.Split(','))
					userList.Add(user);
			}
			return _userManager.Users.Where(u => !userList.Contains(u.UserName) && _nameList.IsValid(u.UserName)).OrderBy(u => u.DisplayName);
		}

		public UsersController(UserManager<SiteUser> userManager, IOptions<NameListOptions> options) {
			_userManager = userManager;
			_nameList = options.Value;
		}
	}
}