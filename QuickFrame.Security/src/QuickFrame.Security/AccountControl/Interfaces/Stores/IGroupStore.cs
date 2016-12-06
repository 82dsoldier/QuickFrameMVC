using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace QuickFrame.Security.AccountControl.Interfaces {

	public interface IGroupStore<TGroup> : IDisposable where TGroup : class {

		Task<string> GetGroupIdAsync(TGroup user, CancellationToken cancellationToken);

		Task<string> GetGroupNameAsync(TGroup user, CancellationToken cancellationToken);

		Task SetGroupNameAsync(TGroup user, string userName, CancellationToken cancellationToken);

		Task<string> GetNormalizedGroupNameAsync(TGroup user, CancellationToken cancellationToken);

		Task SetNormalizedGroupNameAsync(TGroup user, string normalizedName, CancellationToken cancellationToken);

		Task<IdentityResult> CreateAsync(TGroup user, CancellationToken cancellationToken);

		Task<IdentityResult> UpdateAsync(TGroup user, CancellationToken cancellationToken);

		Task<IdentityResult> DeleteAsync(TGroup user, CancellationToken cancellationToken);

		Task<TGroup> FindByIdAsync(string groupId, CancellationToken cancellationToken);

		Task<TGroup> FindByNameAsync(string normalizedGroupName, CancellationToken cancellationToken);
	}
}