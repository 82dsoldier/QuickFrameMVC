using QuickFrame.Data.Interfaces;
using QuickFrame.Security.AccountControl.Data.Dtos;
using QuickFrame.Security.AccountControl.Data.Models;
using System.Collections.Generic;

namespace QuickFrame.Security.AccountControl.Interfaces {

	public interface ISiteGroupsDataService : IDataService<SiteGroup> {

		void RemoveGroupFromRole(int userId, int roleId);

		TEntity GetGroupBySid<TEntity>(string sid);

		bool IsGroupInRole(string sid, string roleName);

		IEnumerable<SiteRoleIndexDto> GetRolesForGroup(string sid);
	}
}