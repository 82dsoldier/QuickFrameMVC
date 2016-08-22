using ExpressMapper;
using Microsoft.AspNetCore.Identity;
using QuickFrame.Di;
using QuickFrame.Security.AccountControl.Data;
using QuickFrame.Security.AccountControl.Data.Dtos;
using QuickFrame.Security.AccountControl.Data.Models;
using QuickFrame.Security.AccountControl.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Security.AccountControl.Services {

	[Export]
	public class SiteRulesDataService : ISiteRulesDataService {
		public void Create<TModel>(TModel model) {
			using(var context = ComponentContainer.Component<SecurityContext>()) {
				var dbModel = Mapper.Map<TModel, SiteRule>(model);
				context.Component.SiteRules.Add(dbModel);
				context.Component.SaveChanges();
			}
		}
		public void Save<TModel>(TModel model) {
			using(var context = ComponentContainer.Component<SecurityContext>()) {
				var dbModel = Mapper.Map<TModel, SiteRule>(model);
				context.Component.SiteRules.Attach(dbModel);
				context.Component.Entry(dbModel).State = EntityState.Modified;
				context.Component.SaveChanges();
			}
		}
		public TResult Get<TResult>(int id) {
			using(var context = ComponentContainer.Component<SecurityContext>()) {
				return Mapper.Map<SiteRule, TResult>(context.Component.SiteRules.First(r => r.Id == id));
			}
		}

		public IEnumerable<TResult> GetList<TResult>() {
			using(var context = ComponentContainer.Component<SecurityContext>()) {
				foreach(var obj in context.Component.SiteRules.Where(obj => obj.IsDeleted == false))
					yield return Mapper.Map<SiteRule, TResult>(obj);
			}
		}
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

		public IEnumerable<SiteGroup> GetGroupsForRule(int id) {
			using(var context = ComponentContainer.Component<SecurityContext>()) {
				using(var groupManager = ComponentContainer.Component<GroupManager<SiteGroup>>()) {
					foreach(var group in context.Component.GroupRules.Where(r => r.RuleId == id)) {
						var t = groupManager.Component.FindByIdAsync(group.GroupId);
						t.Wait();
						yield return t.Result;
					}
				}
			}
		}
		public IEnumerable<SiteUser> GetUsersForRule(int id) {
			using(var context = ComponentContainer.Component<SecurityContext>()) {
				using(var userManager = ComponentContainer.Component<UserManager<SiteUser>>()) {
					foreach(var user in context.Component.UserRules.Where(r => r.RuleId == id)) {
						var t = userManager.Component.FindByIdAsync(user.UserId);
						t.Wait();
						yield return t.Result;
					}
				}
			}
		}
		public void AddUserToRule(int ruleId, string userId) {
			using(var context = ComponentContainer.Component<SecurityContext>()) {
				context.Component.UserRules.Add(new UserRule {
					RuleId = ruleId,
					UserId = userId
				});
				context.Component.SaveChanges();
			}
		}

		public void DeleteUserFromRule(int ruleId, string userId) {
			using(var context = ComponentContainer.Component<SecurityContext>()) {
				var rule = context.Component.UserRules.First(r => r.RuleId == ruleId && r.UserId == userId);
				context.Component.UserRules.Remove(rule);
				context.Component.SaveChanges();
			}
		}

		public void AddGroupToRule(int ruleId, string groupId) {
			using(var context = ComponentContainer.Component<SecurityContext>()) {
				context.Component.GroupRules.Add(new GroupRule {
					RuleId = ruleId,
					GroupId = groupId
				});
				context.Component.SaveChanges();
			}
		}

		public void DeleteGroupFromRule(int ruleId, string groupId) {
			using(var context = ComponentContainer.Component<SecurityContext>()) {
				var rule = context.Component.GroupRules.First(r => r.RuleId == ruleId && r.GroupId == groupId);
				context.Component.GroupRules.Remove(rule);
				context.Component.SaveChanges();
			}
		}
		public void AddRoleToRule(int ruleId, string roleId) {
			using(var context = ComponentContainer.Component<SecurityContext>()) {
				context.Component.RoleRules.Add(new RoleRule {
					RuleId = ruleId,
					RoleId = roleId
				});
				context.Component.SaveChanges();
			}
		}

		public void DeleteRoleFromRule(int ruleId, string roleId) {
			using(var context = ComponentContainer.Component<SecurityContext>()) {
				var rule = context.Component.RoleRules.First(r => r.RuleId == ruleId && r.RoleId == roleId);
				context.Component.RoleRules.Remove(rule);
				context.Component.SaveChanges();
			}
		}
	}
}