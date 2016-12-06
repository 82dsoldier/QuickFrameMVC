using ExpressMapper;
using Microsoft.AspNetCore.Identity;

using QuickFrame.Security.AccountControl.Data;
using QuickFrame.Security.AccountControl.Interfaces.Services;
using QuickFrame.Security.AccountControl.Models;
using System.Collections.Generic;
using System.Linq;
#if NETSTANDARD1_6
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
#endif


namespace QuickFrame.Security.AccountControl.Services {

	public class SiteRulesDataService : ISiteRulesDataService {
		private SecurityContext _context;
		private GroupManager<SiteGroup> _groupManager;
		private UserManager<SiteUser> _userManager;

		public SiteRulesDataService(SecurityContext context, GroupManager<SiteGroup> groupManager, UserManager<SiteUser> userManager) {
			_context = context;
			_groupManager = groupManager;
			_userManager = userManager;
		}

		public void Create<TModel>(TModel model) {
			var dbModel = Mapper.Map<TModel, SiteRule>(model);
			_context.SiteRules.Add(dbModel);
			_context.SaveChanges();
		}

		public void Save<TModel>(TModel model) {
			var dbModel = Mapper.Map<TModel, SiteRule>(model);
			_context.SiteRules.Attach(dbModel);
			_context.Entry(dbModel).State = EntityState.Modified;
			_context.SaveChanges();
		}

		public TResult Get<TResult>(int id) {
			return Mapper.Map<SiteRule, TResult>(_context.SiteRules.First(r => r.Id == id));
		}

		public IEnumerable<TResult> GetList<TResult>() {
			foreach(var obj in _context.SiteRules.Where(obj => obj.IsDeleted == false))
				yield return Mapper.Map<SiteRule, TResult>(obj);
		}

		public IEnumerable<SiteRule> GetSiteRulesForUser(string userId) {
			foreach(var obj in _context.SiteRules.AsNoTracking().Where(obj => obj.IsDeleted == false && obj.UserRules.Any(user => user.UserId == userId))) {
				_context.Entry(obj).State = EntityState.Detached;
				yield return obj;
			}
		}

		public IEnumerable<SiteRule> GetSiteRulesForRole(string roleId) {
			foreach(var obj in _context.SiteRules.Where(obj => obj.IsDeleted == false && obj.SiteRoles.Any(role => role.Id == roleId))) {
				_context.Entry(obj).State = EntityState.Detached;
				yield return obj;
			}
		}

		public IEnumerable<SiteRule> GetSiteRulesForGroup(string gropuId) {
			foreach(var obj in _context.SiteRules.Where(obj => obj.IsDeleted == false && obj.GroupRules.Any(group => group.GroupId == gropuId))) {
				_context.Entry(obj).State = EntityState.Detached;
				yield return obj;
			}
		}

		public IEnumerable<SiteGroup> GetGroupsForRule(int id) {
			foreach(var group in _context.GroupRules.Where(r => r.RuleId == id)) {
				var t = _groupManager.FindByIdAsync(group.GroupId);
				t.Wait();
				yield return t.Result;
			}
		}

		public IEnumerable<SiteUser> GetUsersForRule(int id) {
			foreach(var user in _context.UserRules.Where(r => r.RuleId == id)) {
				var t = _userManager.FindByIdAsync(user.UserId);
				t.Wait();
				yield return t.Result;
			}
		}

		public void AddUserToRule(int ruleId, string userId) {
			_context.UserRules.Add(new UserRule {
				RuleId = ruleId,
				UserId = userId
			});
			_context.SaveChanges();
		}

		public void DeleteUserFromRule(int ruleId, string userId) {
			var rule = _context.UserRules.First(r => r.RuleId == ruleId && r.UserId == userId);
			_context.UserRules.Remove(rule);
			_context.SaveChanges();
		}

		public void AddGroupToRule(int ruleId, string groupId) {
			_context.GroupRules.Add(new GroupRule {
				RuleId = ruleId,
				GroupId = groupId
			});
			_context.SaveChanges();
		}

		public void DeleteGroupFromRule(int ruleId, string groupId) {
			var rule = _context.GroupRules.First(r => r.RuleId == ruleId && r.GroupId == groupId);
			_context.GroupRules.Remove(rule);
			_context.SaveChanges();
		}

		public void AddRoleToRule(int ruleId, string roleId) {
			_context.RoleRules.Add(new RoleRule {
				RuleId = ruleId,
				RoleId = roleId
			});
			_context.SaveChanges();
		}

		public void DeleteRoleFromRule(int ruleId, string roleId) {
			var rule = _context.RoleRules.First(r => r.RuleId == ruleId && r.RoleId == roleId);
			_context.RoleRules.Remove(rule);
			_context.SaveChanges();
		}
	}
}