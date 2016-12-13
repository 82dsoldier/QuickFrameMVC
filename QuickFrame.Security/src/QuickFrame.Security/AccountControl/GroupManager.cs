using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QuickFrame.Security.AccountControl.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QuickFrame.Security.AccountControl {

	//public class GroupManager<TGroup> : IDisposable where TGroup : class {
	//	private bool _disposed;

	//	private readonly HttpContext _context;
	//	private CancellationToken CancellationToken => _context?.RequestAborted ?? CancellationToken.None;
	//	//protected IGroupStore<TGroup> Store { get; set; }
	//	protected ILogger Logger { get; set; }
	//	protected ILookupNormalizer KeyNormalizer { get; set; }
	//	protected QuickFrameIdentityErrorDescriber ErrorDescriber { get; set; }
	//	protected IdentityOptions Options { get; set; }

	//	public virtual bool SupportsGroupRole
	//	{
	//		get
	//		{
	//			ThrowIfDisposed();
	//			return Store is IGroupRoleStore<TGroup>;
	//		}
	//	}

	//	public virtual bool SupportsQueryableGroups
	//	{
	//		get
	//		{
	//			ThrowIfDisposed();
	//			return Store is IQueryableGroupStore<TGroup>;
	//		}
	//	}

	//	public virtual IQueryable<TGroup> Groups
	//	{
	//		get
	//		{
	//			var queryableStore = Store as IQueryableGroupStore<TGroup>;
	//			if(queryableStore == null)
	//				throw new NotSupportedException("The store does not suppport IQueryableGroupStore");
	//			return queryableStore.Groups;
	//		}
	//	}

	//	public virtual async Task<IdentityResult> CreateAsync(TGroup group) {
	//		ThrowIfDisposed();
	//		return await Store.CreateAsync(group, CancellationToken);
	//	}

	//	public virtual Task<IdentityResult> UpdateAsync(TGroup group) {
	//		ThrowIfDisposed();
	//		if(group == null)
	//			throw new ArgumentNullException(nameof(group));
	//		return Store.UpdateAsync(group, CancellationToken);
	//	}

	//	public virtual Task<IdentityResult> DeleteAsync(TGroup group) {
	//		ThrowIfDisposed();
	//		if(group == null)
	//			throw new ArgumentNullException(nameof(group));
	//		return Store.DeleteAsync(group, CancellationToken);
	//	}

	//	public virtual Task<TGroup> FindByIdAsync(string groupId) {
	//		ThrowIfDisposed();
	//		return Store.FindByIdAsync(groupId, CancellationToken);
	//	}

	//	public virtual Task<TGroup> FindByNameAsync(string groupName) {
	//		ThrowIfDisposed();
	//		if(String.IsNullOrEmpty(groupName))
	//			throw new ArgumentNullException(nameof(groupName));

	//		return Store.FindByNameAsync(groupName, CancellationToken);
	//	}

	//	public virtual async Task<string> GetGroupNameAsync(TGroup group) {
	//		ThrowIfDisposed();
	//		if(group == null)
	//			throw new ArgumentNullException(nameof(group));
	//		return await Store.GetGroupNameAsync(group, CancellationToken);
	//	}

	//	public virtual async Task<IdentityResult> SetGroupNameAsync(TGroup group, string groupName) {
	//		ThrowIfDisposed();
	//		if(group == null)
	//			throw new ArgumentNullException(nameof(group));
	//		await Store.SetGroupNameAsync(group, groupName, CancellationToken);
	//		return await UpdateAsync(group);
	//	}

	//	public virtual async Task<IdentityResult> AddToRoleAsync(TGroup group, string role) {
	//		ThrowIfDisposed();
	//		if(group == null)
	//			throw new ArgumentNullException(nameof(group));
	//		var roleStore = GetGroupRoleStore();
	//		var normalizedRole = NormalizeKey(role);
	//		if(await roleStore.IsInRoleAsync(group, normalizedRole, CancellationToken))
	//			return await GroupAlreadyInRoleError(group, role);
	//		await roleStore.AddToRoleAsync(group, normalizedRole, CancellationToken);
	//		return await UpdateAsync(group);
	//	}

	//	public virtual async Task<string> GetGroupIdAsync(TGroup group) {
	//		ThrowIfDisposed();
	//		return await Store.GetGroupIdAsync(group, CancellationToken);
	//	}

	//	public virtual Task<IList<TGroup>> GetGroupsInRoleAsync(string roleName) {
	//		ThrowIfDisposed();
	//		if(String.IsNullOrEmpty(roleName))
	//			throw new ArgumentNullException(nameof(roleName));

	//		return GetGroupRoleStore().GetGroupsInRoleAsync(NormalizeKey(roleName), CancellationToken);
	//	}

	//	public virtual async Task<IdentityResult> AddToRolesAsync(TGroup group, IEnumerable<string> roles) {
	//		ThrowIfDisposed();
	//		if(group == null)
	//			throw new ArgumentNullException(nameof(group));
	//		if(roles == null)
	//			throw new ArgumentNullException(nameof(roles));
	//		var roleStore = GetGroupRoleStore();
	//		foreach(var role in roles.Distinct()) {
	//			var normalizedRole = NormalizeKey(role);
	//			if(await roleStore.IsInRoleAsync(group, normalizedRole, CancellationToken))
	//				return await GroupAlreadyInRoleError(group, role);
	//			await roleStore.AddToRoleAsync(group, normalizedRole, CancellationToken);
	//		}

	//		return await UpdateAsync(group);
	//	}

	//	public virtual async Task<IdentityResult> RemoveFromRoleAsync(TGroup group, string role) {
	//		ThrowIfDisposed();
	//		if(group == null)
	//			throw new ArgumentNullException(nameof(group));
	//		var roleStore = GetGroupRoleStore();
	//		var normalizedRole = NormalizeKey(role);
	//		if(!await roleStore.IsInRoleAsync(group, normalizedRole, CancellationToken))
	//			return await GroupNotInRoleError(group, role);
	//		await roleStore.RemoveFromRoleAsync(group, normalizedRole, CancellationToken);
	//		return await UpdateAsync(group);
	//	}

	//	public virtual async Task<IdentityResult> RemoveFromRolesAsync(TGroup group, IEnumerable<string> roles) {
	//		ThrowIfDisposed();
	//		if(group == null)
	//			throw new ArgumentNullException(nameof(group));
	//		if(roles == null)
	//			throw new ArgumentNullException(nameof(roles));
	//		var roleStore = GetGroupRoleStore();
	//		foreach(var role in roles.Distinct()) {
	//			var normalizedName = NormalizeKey(role);
	//			if(!await roleStore.IsInRoleAsync(group, role, CancellationToken))
	//				return await GroupNotInRoleError(group, role);
	//			await roleStore.RemoveFromRoleAsync(group, role, CancellationToken);
	//		}

	//		return await UpdateAsync(group);
	//	}

	//	public virtual async Task<IList<string>> GetRolesAsync(TGroup group) {
	//		ThrowIfDisposed();
	//		if(group == null)
	//			throw new ArgumentNullException(nameof(group));
	//		return await GetGroupRoleStore().GetRolesAsync(group, CancellationToken);
	//	}

	//	public virtual async Task<bool> IsInRoleAsync(TGroup group, string role) {
	//		ThrowIfDisposed();
	//		if(group == null)
	//			throw new ArgumentNullException(nameof(group));
	//		return await GetGroupRoleStore().IsInRoleAsync(group, NormalizeKey(role), CancellationToken);
	//	}

	//	public virtual string NormalizeKey(string key) {
	//		return (KeyNormalizer == null) ? key : KeyNormalizer.Normalize(key);
	//	}

	//	public void Dispose() {
	//		Dispose(true);
	//		GC.SuppressFinalize(this);
	//	}

	//	public GroupManager(IGroupStore<TGroup> store, IOptions<IdentityOptions> optionsAccessor, ILookupNormalizer keyNormalizer, QuickFrameIdentityErrorDescriber errors, IServiceProvider services, ILogger<GroupManager<TGroup>> logger) {
	//		if(store == null)
	//			throw new ArgumentNullException(nameof(store));

	//		Store = store;
	//		Options = optionsAccessor?.Value ?? new IdentityOptions();
	//		KeyNormalizer = keyNormalizer;
	//		ErrorDescriber = errors;
	//		Logger = logger;

	//		if(services != null)
	//			_context = services.GetService<IHttpContextAccessor>()?.HttpContext;
	//	}

	//	protected void ThrowIfDisposed() {
	//		if(_disposed) {
	//			throw new ObjectDisposedException(GetType().Name);
	//		}
	//	}

	//	protected virtual void Dispose(bool disposing) {
	//		if(disposing && !_disposed) {
	//			Store.Dispose();
	//			_disposed = true;
	//		}
	//	}

	//	private IGroupRoleStore<TGroup> GetGroupRoleStore() {
	//		var cast = Store as IGroupRoleStore<TGroup>;
	//		if(cast == null) {
	//			throw new NotSupportedException("Store does not support roles");
	//		}
	//		return cast;
	//	}

	//	private async Task<IdentityResult> GroupAlreadyInRoleError(TGroup group, string role) {
	//		Logger.LogWarning(5, "Group {groupId} is already in role {role}.", await GetGroupIdAsync(group), role);
	//		return IdentityResult.Failed(ErrorDescriber.GroupAlreadyInRole(role));
	//	}

	//	private async Task<IdentityResult> GroupNotInRoleError(TGroup group, string role) {
	//		Logger.LogWarning(6, "Group {groupId} is not in role {role}.", await GetGroupIdAsync(group), role);
	//		return IdentityResult.Failed(ErrorDescriber.GroupNotInRole(role));
	//	}
	//}
}