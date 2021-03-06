﻿using ExpressMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuickFrame.Data.Dtos;
using QuickFrame.Security.AccountControl;
using QuickFrame.Security.AccountControl.Dtos;
using QuickFrame.Security.AccountControl.Interfaces.Services;
using QuickFrame.Security.AccountControl.Models;
using QuickFrame.Security.Areas.Security.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Security.Areas.Security.Controllers {

	[Area("Security")]
	public class RolesController : Controller {
		private QuickFrameRoleManager _roleManager;
		private UserManager<SiteUser> _userManager;
		//private GroupManager<SiteGroup> _groupManager;
		private ISiteRolesDataService _dataService;

		public IActionResult Index() {
			List<SiteRoleIndexDto> roleList = new List<SiteRoleIndexDto>();
			foreach(var obj in _roleManager.Roles)
				roleList.Add(Mapper.Map<SiteRole, SiteRoleIndexDto>(obj));
			return View(roleList);
		}

		[HttpGet]
		public IActionResult Edit(string id) {
			return View("CreateOrEdit", Mapper.Map<SiteRole, SiteRoleIndexDto>(_roleManager.Roles.First(role => role.Id == id)));
		}

		[HttpPost]
		public IActionResult Edit(SiteRoleIndexDto model) {
			_dataService.Edit(model);
			return View("CreateOrEdit", model);
		}

		[HttpGet]
		public IActionResult Create() {
			return View("CreateOrEdit", new SiteRoleIndexDto());
		}

		[HttpPost]
		public IActionResult Create(SiteRoleIndexDto model) {
			_dataService.Create(model);
			return View("CreateOrEdit", model);
		}

		[HttpDelete]
		public async Task<IActionResult> Delete(string id) {
			return new ObjectResult(await _roleManager.DeleteAsync(_roleManager.Roles.First(obj => obj.Id == id)));
		}

		public async Task<IActionResult> ListUsersInRole(string id) {
			var role = _roleManager.Roles.First(r => r.Id == id);
			var model = new UserRoleIndexModel {
				RoleId = id,
				RoleName = role.Name
			};

			var userList = await _userManager.GetUsersInRoleAsync(id);
			foreach(var user in userList) {
				model.UserList.Add(new UserRoleIndexDto {
					Email = user.Email,
					Phone = user.PhoneNumber,
					RoleId = id,
					RoleName = role.Name,
					UserId = user.Id,
					UserName = user.DisplayName
				});
			}
			return View(model);
		}

		//public async Task<IActionResult> ListGroups(string id) {
		//	var role = _roleManager.Roles.First(r => r.Id == id);
		//	var model = new GroupRoleIndexModel {
		//		RoleId = id,
		//		RoleName = role.Name
		//	};
		//	var groupList = await _groupManager.GetGroupsInRoleAsync(role.Name);
		//	foreach(var group in groupList) {
		//		model.GroupList.Add(new GroupRoleIndexDto {
		//			GroupId = group.Id,
		//			GroupName = group.Name,
		//			RoleId = id,
		//			RoleName = role.Name
		//		});
		//	}
		//	return View(model);
		//}

		[HttpDelete]
		public IActionResult RemoveUserFromRole(string id, string roleId) {
			_dataService.DeleteUser(id, roleId);
			return new ObjectResult(true);
		}

		//[HttpDelete]
		//public async Task<IActionResult> RemoveGroupFromRole(string id, string roleId) {
		//	return new ObjectResult(await _groupManager.RemoveFromRoleAsync((await _groupManager.FindByIdAsync(id)), roleId));
		//}

		[HttpGet]
		public async Task<IActionResult> AddUserToRole(string roleId) {
			HttpContext.Session.SetString("roleId", roleId);
			var userList = (await _userManager.GetUsersInRoleAsync((await _roleManager.FindByIdAsync(roleId)).Name)).OrderBy(u => u.DisplayName).ToList();
			var filter = String.Empty;
			if(userList.Count > 0)
				filter = string.Join(",", userList);
			var model = new UserListModel {
				Controller = "Roles",
				Action = "AddUserToRole",
				Filter = filter,
				Area="Security"
			};
			return View("UserList", model);
		}

		[HttpPost]
		public IActionResult AddUserToRole(UserListModel model) {
			_dataService.AddUser(new UserRole {
				RoleId = HttpContext.Session.GetString("roleId"),
				UserId = model.UserId
			});
			return View("CloseCurrentView");
		}

		//[HttpGet]
		//public async Task<IActionResult> AddGroupToRole(string id) {
		//	HttpContext.Session.SetString("roleId", id);
		//	var groupList = (await _groupManager.GetGroupsInRoleAsync((await _roleManager.FindByIdAsync(id)).Name)).OrderBy(u => u.Name).ToList();
		//	var filter = string.Empty;
		//	if(groupList.Count > 0)
		//		filter = String.Join(",", groupList);
		//	var model = new GroupListModel {
		//		Controller = "Roles",
		//		Action = "AddGroupToRole",
		//		Filter = filter
		//	};
		//	return View("GroupList", model);
		//}

		//[HttpPost]
		//public async Task<IActionResult> AddGroupToRole(GroupListModel model) {
		//	await _groupManager.AddToRoleAsync(await _groupManager.FindByIdAsync(model.GroupId), HttpContext.Session.GetString("roleId"));
		//	return View("CloseCurrentView");
		//}

		[HttpGet]
		public IActionResult GetRoles(string filter = "") {
			var filterList = filter.Split(',').ToList();
			var list = new List<LookupTableDtoString>();
			foreach(var obj in _roleManager.Roles.Where(r => !filterList.Contains(r.Name)))
				list.Add(Mapper.Map<SiteRole, LookupTableDtoString>(obj));
			return new ObjectResult(list);
		}

		public RolesController(QuickFrameRoleManager roleManager, UserManager<SiteUser> userManager, ISiteRolesDataService dataService) {
			_roleManager = roleManager;
			_userManager = userManager;
			//_groupManager = groupManager;
			_dataService = dataService;
		}
	}
}
