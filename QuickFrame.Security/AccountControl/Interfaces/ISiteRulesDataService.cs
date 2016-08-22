using QuickFrame.Security.AccountControl.Data.Models;
using System.Collections.Generic;

namespace QuickFrame.Security.AccountControl.Interfaces {

	public interface ISiteRulesDataService {
		void Create<TModel>(TModel model);
		void Save<TModel>(TModel model);
		TResult Get<TResult>(int id);
		IEnumerable<TResult> GetList<TResult>();
		IEnumerable<SiteRule> GetSiteRulesForUser(string userId);

		IEnumerable<SiteRule> GetSiteRulesForRole(string roleId);

		IEnumerable<SiteRule> GetSiteRulesForGroup(string groupId);
		IEnumerable<SiteUser> GetUsersForRule(int id);
		IEnumerable<SiteGroup> GetGroupsForRule(int id);
		void AddUserToRule(int ruleId, string userId);
		void DeleteUserFromRule(int ruleId, string userId);
		void AddGroupToRule(int ruleId, string groupId);
		void DeleteGroupFromRule(int ruleId, string groupId);
		void AddRoleToRule(int ruleId, string roleId);
		void DeleteRoleFromRule(int ruleId, string roleId);
	}
}