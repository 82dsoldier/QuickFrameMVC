using ExpressMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickFrame.Di;
using QuickFrame.Security.AccountControl.Data;
using QuickFrame.Security.AccountControl.Data.Dtos;
using QuickFrame.Security.AccountControl.Data.Models;
using QuickFrame.Security.AccountControl.Interfaces;
using QuickFrame.Security.Areas.Security.Models;
using System.Collections.Generic;
using System.Linq;

namespace QuickFrame.Security.Areas.Security.Controllers {

	[Area("Security")]
	public class RulesController : Controller {

		public IActionResult Index() {
			using(var context = ComponentContainer.Component<ISiteRulesDataService>()) {
				return View(context.Component.GetList<SiteRuleIndexDto>());
			}
		}

		[HttpGet]
		public IActionResult Create() {
			return View("CreateOrEdit");
		}

		[HttpPost]
		public IActionResult Create(SiteRuleIndexDto model) {
			using(var dataService = ComponentContainer.Component<ISiteRulesDataService>()) {
				dataService.Component.Create(model);
			}
			return View("CloseCurrentView");
		}

		[HttpGet]
		public IActionResult Edit(int id) {
			using(var dataService = ComponentContainer.Component<ISiteRulesDataService>()) {
				return View("CreateOrEdit", dataService.Component.Get<SiteRuleIndexDto>(id));
			}
		}

		[HttpPost]
		public IActionResult Edit(SiteRuleIndexDto model) {
			using(var dataService = ComponentContainer.Component<ISiteRulesDataService>()) {
				dataService.Component.Save(model);
			}
			return View("CloseCurrentView");
		}

		[HttpGet]
		public IActionResult ListUsers(int id) {
			using(var dataService = ComponentContainer.Component<ISiteRulesDataService>()) {
				return View(new UserRuleIndexModel {
					RuleId = id,
					UserList = dataService.Component.GetUsersForRule(id).ToList()
				});
			}
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
			using(var dataService = ComponentContainer.Component<ISiteRulesDataService>()) {
				dataService.Component.AddUserToRule((int)HttpContext.Session.GetInt32("RuleId"), model.UserId);
			}
			return View("CloseCurrentView");
		}

		[HttpDelete]
		public IActionResult RemoveUserFromRule(int id, string userId) {
			using(var dataService = ComponentContainer.Component<ISiteRulesDataService>()) {
				dataService.Component.DeleteUserFromRule(id, userId);
			}
			return new ObjectResult(true);
		}

		[HttpGet]
		public IActionResult ListGroups(int id) {
			using(var dataService = ComponentContainer.Component<ISiteRulesDataService>()) {
				return View(new GroupRuleIndexModel {
					RuleId = id,
					GroupList = dataService.Component.GetGroupsForRule(id).ToList()
				});
			}
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
			using(var dataService = ComponentContainer.Component<ISiteRulesDataService>()) {
				dataService.Component.AddGroupToRule((int)HttpContext.Session.GetInt32("RuleId"), model.GroupId);
			}
			return View("CloseCurrentView");
		}

		[HttpDelete]
		public IActionResult RemoveGroupFromRule(int id, string groupId) {
			using(var dataService = ComponentContainer.Component<ISiteRulesDataService>()) {
				dataService.Component.DeleteGroupFromRule(id, groupId);
			}
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
			using(var dataService = ComponentContainer.Component<ISiteRulesDataService>()) {
				dataService.Component.AddRoleToRule((int)HttpContext.Session.GetInt32("RuleId"), model.RoleId);
			}
			return View("CloseCurrentView");
		}

		[HttpDelete]
		public IActionResult RemoveRoleFromRule(int id, string roleId) {
			using(var dataService = ComponentContainer.Component<ISiteRulesDataService>()) {
				dataService.Component.DeleteRoleFromRule(id, roleId);
			}
			return new ObjectResult(true);
		}
	}
}