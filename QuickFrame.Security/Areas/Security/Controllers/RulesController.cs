using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickFrame.Security.AccountControl.Dtos;
using QuickFrame.Security.AccountControl.Interfaces.Services;
using QuickFrame.Security.Areas.Security.Models;
using System.Linq;

namespace QuickFrame.Security.Areas.Security.Controllers {

	[Area("Security")]
	public class RulesController : Controller {
		private ISiteRulesDataService _siteRulesDataService;

		public RulesController(ISiteRulesDataService siteRulesDataService) {
			_siteRulesDataService = siteRulesDataService;
		}

		public IActionResult Index() {
			return View(_siteRulesDataService.GetList<SiteRuleIndexDto>());
		}

		[HttpGet]
		public IActionResult Create() {
			return View("CreateOrEdit");
		}

		[HttpPost]
		public IActionResult Create(SiteRuleIndexDto model) {
			_siteRulesDataService.Create(model);
			return View("CloseCurrentView");
		}

		[HttpGet]
		public IActionResult Edit(int id) {
			return View("CreateOrEdit", _siteRulesDataService.Get<SiteRuleIndexDto>(id));
		}

		[HttpPost]
		public IActionResult Edit(SiteRuleIndexDto model) {
			_siteRulesDataService.Save(model);
			return View("CloseCurrentView");
		}

		[HttpGet]
		public IActionResult ListUsers(int id) {
			return View(new UserRuleIndexModel {
				RuleId = id,
				UserList = _siteRulesDataService.GetUsersForRule(id).ToList()
			});
		}

		[HttpGet]
		public IActionResult AddUserToRule(int id) {
			HttpContext.Session.SetInt32("RuleId", id);
			return View("UserList", new UserListModel {
				Action = "AddUserToRule",
				Controller = "Rules",
				Area = "Security"
			});
		}

		[HttpPost]
		public IActionResult AddUserToRule(UserListModel model) {
			_siteRulesDataService.AddUserToRule((int)HttpContext.Session.GetInt32("RuleId"), model.UserId);
			return View("CloseCurrentView");
		}

		[HttpDelete]
		public IActionResult RemoveUserFromRule(int id, string userId) {
			_siteRulesDataService.DeleteUserFromRule(id, userId);
			return new ObjectResult(true);
		}

		[HttpGet]
		public IActionResult ListGroups(int id) {
			return View(new GroupRuleIndexModel {
				RuleId = id,
				GroupList = _siteRulesDataService.GetGroupsForRule(id).ToList()
			});
		}

		[HttpGet]
		public IActionResult AddGroupToRule(int id) {
			HttpContext.Session.SetInt32("RuleId", id);
			return View("GroupList", new GroupListModel {
				Action = "AddGroupToRule",
				Controller = "Rules",
				Area = "Security"
			});
		}

		[HttpPost]
		public IActionResult AddGroupToRule(GroupListModel model) {
			_siteRulesDataService.AddGroupToRule((int)HttpContext.Session.GetInt32("RuleId"), model.GroupId);
			return View("CloseCurrentView");
		}

		[HttpDelete]
		public IActionResult RemoveGroupFromRule(int id, string groupId) {
			_siteRulesDataService.DeleteGroupFromRule(id, groupId);
			return new ObjectResult(true);
		}

		[HttpGet]
		public IActionResult AddRoleToRule(int id) {
			HttpContext.Session.SetInt32("RuleId", id);
			return View("RoleList", new RoleListModel {
				Action = "AddGroupToRule",
				Controller = "Rules",
				Area = "Security"
			});
		}

		[HttpPost]
		public IActionResult AddRoleToRule(RoleListModel model) {
			_siteRulesDataService.AddRoleToRule((int)HttpContext.Session.GetInt32("RuleId"), model.RoleId);
			return View("CloseCurrentView");
		}

		[HttpDelete]
		public IActionResult RemoveRoleFromRule(int id, string roleId) {
			_siteRulesDataService.DeleteRoleFromRule(id, roleId);
			return new ObjectResult(true);
		}
	}
}