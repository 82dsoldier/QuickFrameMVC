using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QuickFrame.Security.AccountControl;
using QuickFrame.Security.AccountControl.Models;
using QuickFrame.Security.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuickFrame.Security.Areas.Security.Controllers {

	//[Area("Security")]
	//public class GroupsController : Controller {
	//	private GroupManager<SiteGroup> _groupManager;
	//	private NameListOptions _nameList;

	//	[HttpGet]
	//	public IEnumerable<SiteGroup> GetGroups(string filter = "") {
	//		var groupList = new List<string>();
	//		if(!String.IsNullOrEmpty(filter)) {
	//			foreach(var group in filter.Split(','))
	//				groupList.Add(group);
	//		}
	//		return _groupManager.Groups.Where(u => !groupList.Contains(u.Name) && _nameList.IsValid(u.Name)).OrderBy(u => u.Name);
	//	}

	//	public GroupsController(GroupManager<SiteGroup> groupManager, IOptions<NameListOptions> options) {
	//		_groupManager = groupManager;
	//		_nameList = options.Value;
	//	}
	//}
}