using QuickFrame.Data.Services;
using QuickFrame.Di;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System;
using System.Data.SqlClient;
using QuickFrame.Security.AccountControl.Data;
using QuickFrame.Security.AccountControl.Data.Models;
using QuickFrame.Security.AccountControl.Interfaces;

namespace QuickFrame.Security.Data.Services {

	[Export]
	public class SiteRolesDataService : DataService<PermissionsContext, SiteRole>, ISiteRolesDataService {
		public IEnumerable<string> GetRolesForUser(string userId) {
			using(var context = ComponentContainer.Component<PermissionsContext>().Component) {
				var user = context.SiteUsers.First(u => u.UserId == userId);

				if(user == null)
					yield break;

				foreach(var role in user.Roles)
					yield return role.Name;

				if(user.Groups != null) {
					foreach(var group in user.Groups)
						foreach(var role in group.Roles)
							yield return role.Name;
				}
			}
		}

		public IEnumerable<string> GetRolesForGroup(string groupId) {
			using(var context = ComponentContainer.Component<PermissionsContext>().Component) {
				var user = context.SiteGroups.FirstOrDefault(g => g.GroupId == groupId);

				if(user == null)
					yield break;

				foreach(var role in user.Roles)
					yield return role.Name;
			}
		}
	}
}