using Microsoft.AspNet.Mvc;
using QuickFrame.Mvc;
using QuickFrame.Security.Data.Dtos;
using QuickFrame.Security.Data.Interfaces;
using QuickFrame.Security.Data.Models;

namespace QuickFrame.Security.AccountControl.Controllers {

	public abstract class SiteGroupsController : ControllerBase<SiteGroup, SiteGroupIndexDto, SiteGroupEditDto> {

		[Route("GetGroupList", Name = "GetGroupList")]
		[HttpGet]
		public IActionResult GetGroupList() => Authorize(User, () => GetGroups());

		[HttpDelete("{userId}/{roleId}")]
		public void RemoveGroupFromRole(int groupId, int roleId) {
			(_dataService as ISiteGroupsDataService)?.RemoveGroupFromRole(groupId, roleId);
		}

		protected abstract IActionResult GetGroups();

		public SiteGroupsController(ISiteGroupsDataService data)
			: base(data) {
		}
	}
}