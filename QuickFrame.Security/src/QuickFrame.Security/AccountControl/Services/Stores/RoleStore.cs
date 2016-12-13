using Microsoft.AspNetCore.Identity;
using QuickFrame.Security.AccountControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using QuickFrame.Security.AccountControl.Data;
using System.Data.Entity;

namespace QuickFrame.Security.AccountControl.Stores
{
	public class RoleStore : IRoleStore<SiteRole>, IQueryableRoleStore<SiteRole> {
		SecurityContext _context;

		public RoleStore(SecurityContext context) {
			_context = context;
		}

		public IQueryable<SiteRole> Roles
		{
			get
			{
				return _context.SiteRoles;
			}
		}

		public Task<IdentityResult> CreateAsync(SiteRole role, CancellationToken cancellationToken) {
			_context.SiteRoles.Add(role);
			if(cancellationToken.IsCancellationRequested)
				return Task.FromResult(IdentityResult.Failed(new[] { new IdentityError() { Description = "Operation was cancelled by request" } }));
			_context.SaveChanges();
			return Task.FromResult(IdentityResult.Success);
		}

		public Task<IdentityResult> DeleteAsync(SiteRole role, CancellationToken cancellationToken) {
			_context.SiteRoles.Remove(role);
			if(cancellationToken.IsCancellationRequested)
				return Task.FromResult(IdentityResult.Failed(new[] { new IdentityError() { Description = "Operation was cancelled by request" } }));
			_context.SaveChanges();
			return Task.FromResult(IdentityResult.Success);
		}

		public void Dispose() {
		}

		public Task<SiteRole> FindByIdAsync(string roleId, CancellationToken cancellationToken) {
			return Task.FromResult(_context.SiteRoles.FirstOrDefault(r => r.Id == roleId));
		}

		public Task<SiteRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken) {
			return Task.FromResult(_context.SiteRoles.FirstOrDefault(r => r.NormalizedName == normalizedRoleName));
		}

		public Task<string> GetNormalizedRoleNameAsync(SiteRole role, CancellationToken cancellationToken) {
			if(!String.IsNullOrEmpty(role.NormalizedName))
				return Task.FromResult(role.NormalizedName);
			if(!String.IsNullOrEmpty(role.Id))
				return Task.FromResult(_context.SiteRoles.First(r => r.Id == role.Id).NormalizedName);
			if(!String.IsNullOrEmpty(role.Name))
				return Task.FromResult(_context.SiteRoles.First(r => r.Name == role.Name).NormalizedName);
			throw new ArgumentException("Specified role was not found");
		}

		public Task<string> GetRoleIdAsync(SiteRole role, CancellationToken cancellationToken) {
			if(!String.IsNullOrEmpty(role.Id))
				return Task.FromResult(role.Id);
			if(!String.IsNullOrEmpty(role.Name))
				return Task.FromResult(_context.SiteRoles.First(r => r.Name == role.Name).Id);
			if(!String.IsNullOrEmpty(role.NormalizedName))
				return Task.FromResult(_context.SiteRoles.First(r => r.NormalizedName == role.NormalizedName).Id);
			throw new ArgumentException("Specified role was not found");
		}

		public Task<string> GetRoleNameAsync(SiteRole role, CancellationToken cancellationToken) {
			if(!String.IsNullOrEmpty(role.Name))
				return Task.FromResult(role.Name);
			if(!String.IsNullOrEmpty(role.Id))
				return Task.FromResult(_context.SiteRoles.First(r => r.Id == role.Id).Name);
			if(!String.IsNullOrEmpty(role.NormalizedName))
				return Task.FromResult(_context.SiteRoles.First(r => r.NormalizedName == role.NormalizedName).Name);
			throw new ArgumentException("Specified role was not found");
		}

		public Task SetNormalizedRoleNameAsync(SiteRole role, string normalizedName, CancellationToken cancellationToken) {
			role.NormalizedName = normalizedName;
			return UpdateAsync(role, cancellationToken);
		}

		public Task SetRoleNameAsync(SiteRole role, string roleName, CancellationToken cancellationToken) {
			role.Name = roleName;
			return UpdateAsync(role, cancellationToken);
		}

		public Task<IdentityResult> UpdateAsync(SiteRole role, CancellationToken cancellationToken) {
			_context.SiteRoles.Attach(role);
			_context.Entry(role).State = EntityState.Modified;
			if(cancellationToken.IsCancellationRequested)
				return Task.FromResult(IdentityResult.Failed(new[] { new IdentityError() { Description = "Operation was cancelled by request" } }));
			_context.SaveChanges();
			return Task.FromResult(IdentityResult.Success);
		}
	}
}
