using QuickFrame.Di;
using QuickFrame.Security.AccountControl.Data;
using QuickFrame.Security.AccountControl.Data.Models;
using QuickFrame.Security.AccountControl.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace QuickFrame.Security.AccountControl.Services {

	[Export]
	public class SiteRulesDataService : ISiteRulesDataService {

		public IEnumerable<SiteRule> GetSiteRulesForUser(string userId) {
			using(var context = ComponentContainer.Component<SecurityContext>()) {
				foreach(var obj in context.Component.SiteRules.Where(obj => obj.IsDeleted == false && obj.UserRules.Any(user => user.UserId == userId))) {
					((IObjectContextAdapter)context.Component).ObjectContext.Detach(obj);
					yield return obj;
				}
			}
		}

		public IEnumerable<SiteRule> GetSiteRulesForRole(string roleId) {
			using(var context = ComponentContainer.Component<SecurityContext>()) {
				foreach(var obj in context.Component.SiteRules.Where(obj => obj.IsDeleted == false && obj.SiteRoles.Any(role => role.Id == roleId))) {
					((IObjectContextAdapter)context.Component).ObjectContext.Detach(obj);
					yield return obj;
				}
			}
		}

		public IEnumerable<SiteRule> GetSiteRulesForGroup(string gropuId) {
			using(var context = ComponentContainer.Component<SecurityContext>()) {
				foreach(var obj in context.Component.SiteRules.Where(obj => obj.IsDeleted == false && obj.GroupRules.Any(group => group.GroupId == gropuId))) {
					((IObjectContextAdapter)context.Component).ObjectContext.Detach(obj);
					yield return obj;
				}
			}
		}
	}
}