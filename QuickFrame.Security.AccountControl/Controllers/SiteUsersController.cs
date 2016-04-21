using Microsoft.AspNet.Mvc;
using QuickFrame.Mvc;
using QuickFrame.Security.Data.Dtos;
using QuickFrame.Security.Data.Interfaces;
using QuickFrame.Security.Data.Models;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Security.AccountControl.Controllers {

	public abstract class SiteUsersController : ControllerBase<SiteUser, SiteUserIndexDto, SiteUserEditDto> {

		[Route("GetUserList", Name = "GetUserList")]
		[HttpGet]
		public IActionResult GetUserList() => Authorize(User, () => GetUsers());

		[HttpDelete("{userId}/{roleId}")]
		public void RemoveUserFromRole(int userId, int roleId) {
			(_dataService as ISiteUsersDataService)?.RemoveUserFromRole(userId, roleId);
		}

		protected abstract IActionResult GetUsers();

		public SiteUsersController(ISiteUsersDataService data)
			: base(data) {
		}
	}

	internal class UserObject {
		public string Name { get; set; }
		public string Id { get; set; }
	}
}