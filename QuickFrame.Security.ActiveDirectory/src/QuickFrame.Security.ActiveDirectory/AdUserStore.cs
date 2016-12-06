using ExpressMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using QuickFrame.Data;
using QuickFrame.Security.AccountControl.ActiveDirectory.AdLookup;
using QuickFrame.Security.AccountControl.Data;
using QuickFrame.Security.AccountControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QuickFrame.Security.AccountControl.ActiveDirectory {

	public class AdUserStore : IUserStore<SiteUser>, IUserRoleStore<SiteUser>, IQueryableUserStore<SiteUser> {
		private SecurityContext _context;
		public IQueryable<SiteUser> Users { get { return Mapper.Map<IEnumerable<Person>, IEnumerable<SiteUser>>(AccountService.GetAccounts()).AsQueryable(); } }

		public Task AddToRoleAsync(SiteUser user, string roleName, CancellationToken cancellationToken) {
			var userRole = new UserRole();
			userRole.RoleId = user.Id;
			userRole.RoleId = _context.SiteRoles.First(r => r.Name == roleName).Id;
			_context.UserRoles.Add(userRole);
			return Task.FromResult(_context.SaveChanges());
		}

		public Task<IdentityResult> CreateAsync(SiteUser user, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task<IdentityResult> DeleteAsync(SiteUser user, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public void Dispose() {
			//_searcher.Dispose();
		}

		public Task<SiteUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
			=> Task.FromResult(Mapper.Map<Person, SiteUser>(AccountService.GetAccount(userId)));

		public Task<SiteUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
			=> Task.FromResult(Mapper.Map<Person, SiteUser>(AccountService.GetAccountByUserName(normalizedUserName)));

		public Task<string> GetNormalizedUserNameAsync(SiteUser user, CancellationToken cancellationToken) => Task.FromResult(user.NormalizedUserName);

		public Task<IList<string>> GetRolesAsync(SiteUser user, CancellationToken cancellationToken) {
			return Task.FromResult((from role in _context.SiteRoles
									where role.UserRoles.Any(ur => ur.UserId == user.Id)
									select role.Name).ToList() as IList<string>);
		}

		public Task<string> GetUserIdAsync(SiteUser user, CancellationToken cancellationToken) => Task.FromResult(user.Id);

		public Task<string> GetUserNameAsync(SiteUser user, CancellationToken cancellationToken) => Task.FromResult(user.UserName);

		public Task<IList<SiteUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken) {
			IEnumerable<string> userList = GetUserIdsInRole(roleName);
			var obj = Mapper.Map<IEnumerable<Person>, IEnumerable<SiteUser>>(AccountService.GetAccounts().Where(account => userList.Any(o => o == account.Id)));
			return Task.FromResult(obj as IList<SiteUser>);
		}

		public Task<bool> IsInRoleAsync(SiteUser user, string roleName, CancellationToken cancellationToken) {
			IEnumerable<string> userList = GetUserIdsInRole(roleName);
			return Task.FromResult(userList.Contains(user.Id));
		}

		public Task RemoveFromRoleAsync(SiteUser user, string roleName, CancellationToken cancellationToken) {
			var role = (_context.SiteRoles.First(siteRole => siteRole.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)));
			var userRole = _context.UserRoles.First(r => r.UserId == user.Id && r.RoleId == role.Id);
			_context.UserRoles.Remove(userRole);
			return Task.FromResult(_context.SaveChanges());
		}

		public Task SetNormalizedUserNameAsync(SiteUser user, string normalizedName, CancellationToken cancellationToken) {
			user.NormalizedUserName = normalizedName;
			return Task.FromResult(true);
		}

		public Task SetUserNameAsync(SiteUser user, string userName, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task<IdentityResult> UpdateAsync(SiteUser user, CancellationToken cancellationToken) {
			return Task.FromResult(IdentityResult.Success);
		}

		private IEnumerable<string> GetUserIdsInRole(string roleName) {
			foreach(var obj in from userRole in _context.UserRoles
							   where userRole.RoleId ==
								   (from role in _context.SiteRoles
									where role.NormalizedName == roleName
									select role.Id).FirstOrDefault()
							   select userRole.UserId) {
				yield return obj;
			}
		}

		public AdUserStore(IOptions<DataOptions> options, SecurityContext context) {
			AccountService.SearchPath = options.Value.ConnectionString.AdSecurity;
			_context = context;
		}
	}
}