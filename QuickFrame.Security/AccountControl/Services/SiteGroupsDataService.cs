using ExpressMapper;
using QuickFrame.Data.Services;
using QuickFrame.Di;
using QuickFrame.Security.AccountControl.Interfaces;
using System.Collections.Generic;
using System.Composition;
using System.Data.Entity;
using System.Linq;
using QuickFrame.Security.AccountControl.Data;
using QuickFrame.Security.AccountControl.Data.Models;
using QuickFrame.Security.AccountControl.Data.Dtos;

namespace QuickFrame.Security.AccountControl.Services {

	[Export]
	public class SiteGroupsDataService : DataService<PermissionsContext, SiteGroup>, ISiteGroupsDataService {

		public override void Save<TModel>(TModel model) {
			using(var contextFactory = ComponentContainer.Component<PermissionsContext>()) {
				var dbModel = model as SiteGroupEditDto;
				var group = contextFactory.Component.SiteGroups.Include(g => g.Roles).FirstOrDefault(g => g.GroupId == dbModel.GroupId);
				if(group == null) {
					group = Mapper.Map<SiteGroupEditDto, SiteGroup>(dbModel);
					contextFactory.Component.SiteGroups.Add(group);
				}

				if(group.Roles == null)
					group.Roles = new List<SiteRole>();

				group.Roles.Add(contextFactory.Component.SiteRoles.FirstOrDefault(r => r.Id == dbModel.RoleId));
				contextFactory.Component.SaveChanges();
			}
		}

		public void RemoveGroupFromRole(int groupId, int roleId) {
			using(var contextFactory = ComponentContainer.Component<PermissionsContext>()) {
				var group =
				contextFactory.Component.SiteGroups.Include(u => u.Roles)
					.FirstOrDefault(u => u.Id == groupId && u.Roles.Any(r => r.Id == roleId));
				if(group != null) {
					group.Roles.Remove(group.Roles.First(r => r.Id == roleId));
					contextFactory.Component.Entry(group).State = EntityState.Modified;
					contextFactory.Component.SaveChanges();
				}
			}
		}

		public TResult GetGroupBySid<TResult>(string sid) {
			using(var contextFactory = ComponentContainer.Component<PermissionsContext>()) {
				var group = contextFactory.Component.SiteGroups.FirstOrDefault(obj => obj.GroupId == sid);
				return group != null ? Mapper.Map<SiteGroup, TResult>(group) : default(TResult);
			}
		}

		public bool IsGroupInRole(string sid, string roleName) {
			using(var contextFactory = ComponentContainer.Component<PermissionsContext>()) {
				return contextFactory.Component.SiteGroups.Any(g => g.GroupId == sid && g.Roles.Any(r => r.Name == roleName));
			}
		}

		public IEnumerable<SiteRoleIndexDto> GetRolesForGroup(string sid) {
			using(var contextFactory = ComponentContainer.Component<PermissionsContext>()) {
				var query = contextFactory.Component.SiteGroups.Include(u => u.Roles);
				var obj = query.FirstOrDefault(u => u.GroupId == sid); //?.Roles;

				if(obj == null)
					yield break;
				foreach(var role in obj.Roles)
					yield return Mapper.Map<SiteRole, SiteRoleIndexDto>(role);
			}
		}
	}
}