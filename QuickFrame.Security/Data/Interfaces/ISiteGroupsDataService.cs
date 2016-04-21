using QuickFrame.Data.Interfaces;
using QuickFrame.Security.Data.Dtos;
using QuickFrame.Security.Data.Models;
using System.Collections.Generic;

namespace QuickFrame.Security.Data.Interfaces {

	public interface ISiteGroupsDataService : IDataService<SiteGroup> {

		void RemoveGroupFromRole(int userId, int roleId);

		TEntity GetGroupBySid<TEntity>(string sid);

		bool IsGroupInRole(string sid, string roleName);

		IEnumerable<SiteRoleIndexDto> GetRolesForGroup(string sid);
	}
}