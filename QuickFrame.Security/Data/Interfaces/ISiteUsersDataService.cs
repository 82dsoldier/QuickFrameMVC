using QuickFrame.Data.Interfaces;
using QuickFrame.Security.Data.Dtos;
using QuickFrame.Security.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Security.Data.Interfaces
{
	public interface ISiteUsersDataService : IDataService<SiteUser> {
		void RemoveUserFromRole(int userId, int roleId);
		TEntity GetUserBySid<TEntity>(string sid);
		bool IsUserInRole(string sid, string roleName);

		IEnumerable<SiteRoleIndexDto> GetRolesForUser(string sid);
	}
}
