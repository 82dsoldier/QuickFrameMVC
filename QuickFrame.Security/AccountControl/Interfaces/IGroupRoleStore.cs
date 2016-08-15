using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace QuickFrame.Security.AccountControl.Interfaces {

	public interface IGroupRoleStore<TGroup> where TGroup : class {

		Task AddToRoleAsync(TGroup group, string roleName, CancellationToken cancellationToken);

		Task RemoveFromRoleAsync(TGroup group, string roleName, CancellationToken cancellationToken);

		Task<IList<string>> GetRolesAsync(TGroup group, CancellationToken cancellationToken);

		Task<bool> IsInRoleAsync(TGroup group, string roleName, CancellationToken cancellationToken);

		Task<IList<TGroup>> GetGroupsInRoleAsync(string roleName, CancellationToken cancellationToken);
	}
}