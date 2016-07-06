using QuickFrame.Data.Interfaces;
using QuickFrame.Security.AccountControl.Data.Dtos;
using QuickFrame.Security.AccountControl.Data.Models;
using System.Collections.Generic;

namespace QuickFrame.Security.AccountControl.Interfaces {

	public interface ISiteUsersDataService : IDataService<SiteUser> {

		void RemoveUserFromRole(int userId, int roleId);

		TEntity GetUserBySid<TEntity>(string sid);

		bool IsUserInRole(string sid, string roleName);

		IEnumerable<SiteRoleIndexDto> GetRolesForUser(string sid);

		IEnumerable<SiteGroupIndexDto> GetGroupsForUser(string sid);
	}
}