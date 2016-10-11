using ExpressMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using QuickFrame.Data;
using QuickFrame.Security.AccountControl.ActiveDirectory.AdLookup;
using QuickFrame.Security.AccountControl.Data;
using QuickFrame.Security.AccountControl.Interfaces;
using QuickFrame.Security.AccountControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QuickFrame.Security.AccountControl.ActiveDirectory {

	public class AdGroupStore : IGroupStore<SiteGroup>, IGroupRoleStore<SiteGroup>, IQueryableGroupStore<SiteGroup> {
		private SecurityContext _context;

		public AdGroupStore(IOptions<DataOptions> options, SecurityContext context) {
			_context = context;
			GroupService.SearchPath = options.Value.ConnectionString.AdSecurity;
		}

		public IQueryable<SiteGroup> Groups
		{
			get
			{
				var returnList = new List<SiteGroup>();
				foreach(var obj in GroupService.GetGroups())
					returnList.Add(Mapper.Map<Group, SiteGroup>(obj));
				return returnList.AsQueryable();
			}
		}

		public Task AddToRoleAsync(SiteGroup group, string roleName, CancellationToken cancellationToken) {
			var groupRole = new GroupRole();
			groupRole.GroupId = group.Id;
			groupRole.RoleId = _context.SiteRoles.First(r => r.Name == roleName).Id;
			_context.GroupRoles.Add(groupRole);
			return Task.FromResult(_context.SaveChanges());
		}

		public Task<IdentityResult> CreateAsync(SiteGroup user, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task<IdentityResult> DeleteAsync(SiteGroup user, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public void Dispose() {
		}

		public Task<SiteGroup> FindByIdAsync(string groupId, CancellationToken cancellationToken)
			=> Task.FromResult(Mapper.Map<Group, SiteGroup>(GroupService.GetGroup(groupId)));

		public Task<SiteGroup> FindByNameAsync(string normalizedGroupName, CancellationToken cancellationToken)
			=> Task.FromResult(Mapper.Map<Group, SiteGroup>(GroupService.GetGroupByGroupName(normalizedGroupName)));

		public Task<string> GetGroupIdAsync(SiteGroup group, CancellationToken cancellationToken) => Task.FromResult(group.Id);

		public Task<string> GetGroupNameAsync(SiteGroup group, CancellationToken cancellationToken) => Task.FromResult(group.Name);

		public Task<IList<SiteGroup>> GetGroupsInRoleAsync(string roleName, CancellationToken cancellationToken) {
			var groupList = GetGroupIdsInRole(roleName);
			var obj = Mapper.Map<IEnumerable<Group>, IEnumerable<SiteGroup>>(GroupService.GetGroups().Where(account => groupList.Any(o => o == account.CommonName)));
			return Task.FromResult(obj as IList<SiteGroup>);
		}

		public Task<string> GetNormalizedGroupNameAsync(SiteGroup group, CancellationToken cancellationToken) => Task.FromResult(group.Name.ToUpper());

		public Task<IList<string>> GetRolesAsync(SiteGroup siteGroup, CancellationToken cancellationToken) {
			return Task.FromResult((from role in _context.SiteRoles
									where role.GroupRoles.Any(ur => ur.GroupId == siteGroup.Id)
									select role.Name).ToList() as IList<string>);
		}

		public Task<bool> IsInRoleAsync(SiteGroup group, string roleName, CancellationToken cancellationToken) {
			IEnumerable<string> groupList = GetGroupIdsInRole(roleName);
			return Task.FromResult(groupList.Contains(group.Id));
		}

		public Task RemoveFromRoleAsync(SiteGroup group, string roleName, CancellationToken cancellationToken) {
			var role = (_context.SiteRoles.First(siteRole => siteRole.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)));
			var groupRole = _context.GroupRoles.First(r => r.GroupId == group.Id && r.RoleId == role.Id);
			_context.GroupRoles.Remove(groupRole);
			return Task.FromResult(_context.SaveChanges());
		}

		public Task SetGroupNameAsync(SiteGroup user, string userName, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task SetNormalizedGroupNameAsync(SiteGroup user, string normalizedName, CancellationToken cancellationToken) {
			return Task.FromResult(true);
		}

		public Task<IdentityResult> UpdateAsync(SiteGroup user, CancellationToken cancellationToken) {
			return Task.FromResult(IdentityResult.Success);
		}

		private IEnumerable<string> GetGroupIdsInRole(string roleName) {
			foreach(var obj in from groupRole in _context.GroupRoles
							   where groupRole.RoleId ==
								   (from role in _context.SiteRoles
									where role.NormalizedName == roleName
									select role.Id).FirstOrDefault()
							   select groupRole.GroupId) {
				yield return obj;
			}
		}
	}
}