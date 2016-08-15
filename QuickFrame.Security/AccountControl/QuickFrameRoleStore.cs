using Microsoft.AspNetCore.Identity;
using QuickFrame.Di;
using QuickFrame.Security.AccountControl.Data;
using QuickFrame.Security.AccountControl.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QuickFrame.Security.AccountControl {

	public class QuickFrameRoleStore : IRoleStore<SiteRole>, IQueryableRoleStore<SiteRole> {

		public IQueryable<SiteRole> Roles
		{
			get
			{
				using(var context = ComponentContainer.Component<SecurityContext>()) {
					List<SiteRole> roleList = new List<SiteRole>();
					foreach(var obj in context.Component.SiteRoles) {
						((IObjectContextAdapter)context.Component).ObjectContext.Detach(obj);
						roleList.Add(obj);
					}

					return roleList.AsQueryable();
				}
			}
		}

		public Task<IdentityResult> CreateAsync(SiteRole role, CancellationToken cancellationToken) {
			try {
				using(var context = ComponentContainer.Component<SecurityContext>()) {
					context.Component.SiteRoles.Add(role);
					context.Component.SaveChanges();
				}
			} catch(Exception e) {
				return Task.FromResult(IdentityResult.Failed(new IdentityError[] { new IdentityError { Code = e.HResult.ToString(), Description = e.Message } }));
			}
			return Task.FromResult(IdentityResult.Success);
		}

		public Task<IdentityResult> DeleteAsync(SiteRole role, CancellationToken cancellationToken) {
			try {
				using(var context = ComponentContainer.Component<SecurityContext>()) {
					context.Component.SiteRoles.Remove(role);
					context.Component.SaveChanges();
				}
			} catch(Exception e) {
				return Task.FromResult(IdentityResult.Failed(new IdentityError[] { new IdentityError { Code = e.HResult.ToString(), Description = e.Message } }));
			}
			return Task.FromResult(IdentityResult.Success);
		}

		public void Dispose() {
		}

		public Task<SiteRole> FindByIdAsync(string roleId, CancellationToken cancellationToken) {
			using(var context = ComponentContainer.Component<SecurityContext>()) {
				var role = context.Component.SiteRoles.FirstOrDefault(obj => obj.Id == roleId);
				((IObjectContextAdapter)context.Component).ObjectContext.Detach(role);
				return Task.FromResult(role);
			}
		}

		public Task<SiteRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken) {
			using(var context = ComponentContainer.Component<SecurityContext>()) {
				var obj = context.Component.SiteRoles.FirstOrDefault(role => role.NormalizedName == normalizedRoleName);
				((IObjectContextAdapter)context.Component).ObjectContext.Detach(obj);
				return Task.FromResult(obj);
			}
		}

		public Task<string> GetNormalizedRoleNameAsync(SiteRole role, CancellationToken cancellationToken) => Task.FromResult(role.NormalizedName);

		public Task<string> GetRoleIdAsync(SiteRole role, CancellationToken cancellationToken) => Task.FromResult(role.Id);

		public Task<string> GetRoleNameAsync(SiteRole role, CancellationToken cancellationToken) => Task.FromResult(role.Name);

		public Task SetNormalizedRoleNameAsync(SiteRole role, string normalizedName, CancellationToken cancellationToken) {
			using(var context = ComponentContainer.Component<SecurityContext>()) {
				role.NormalizedName = normalizedName;
				context.Component.SiteRoles.Attach(role);
				context.Component.Entry(role).State = EntityState.Modified;
				var ret = context.Component.SaveChanges();
				return Task.FromResult(ret);
			}
		}

		public Task SetRoleNameAsync(SiteRole role, string roleName, CancellationToken cancellationToken) {
			using(var context = ComponentContainer.Component<SecurityContext>()) {
				role.Name = roleName;
				context.Component.SiteRoles.Attach(role);
				context.Component.Entry(role).State = EntityState.Modified;
				var result = context.Component.SaveChanges();
				return Task.FromResult(result);
			}
		}

		public Task<IdentityResult> UpdateAsync(SiteRole role, CancellationToken cancellationToken) {
			try {
				using(var context = ComponentContainer.Component<SecurityContext>()) {
					context.Component.SiteRoles.Attach(role);
					context.Component.Entry(role).State = EntityState.Modified;
					context.Component.SaveChanges();
				}
			} catch(Exception e) {
				return Task.FromResult(IdentityResult.Failed(new IdentityError[] { new IdentityError { Code = e.HResult.ToString(), Description = e.Message } }));
			}
			return Task.FromResult(IdentityResult.Success);
		}

		public QuickFrameRoleStore() {
		}
	}
}