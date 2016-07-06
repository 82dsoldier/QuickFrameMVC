﻿using ExpressMapper;
using QuickFrame.Data.Services;
using QuickFrame.Di;
using System.Collections.Generic;
using System.Composition;
using System.Data.Entity;
using System.Linq;
using System;
using System.Data.SqlClient;
using QuickFrame.Security.AccountControl.Data;
using QuickFrame.Security.AccountControl.Data.Models;
using QuickFrame.Security.AccountControl.Interfaces;
using QuickFrame.Security.AccountControl.Data.Dtos;

namespace QuickFrame.Security.Data.Services {

	[Export]
	public class SiteUsersDataService : DataService<PermissionsContext, SiteUser>, ISiteUsersDataService {

		public override void Save<TModel>(TModel model) {
			using(var contextFactory = ComponentContainer.Component<PermissionsContext>()) {
				var dbModel = model as SiteUserEditDto;
				var user = contextFactory.Component.SiteUsers.Include(u => u.Roles).FirstOrDefault(u => u.UserId == dbModel.UserId);
				if(user == null) {
					user = Mapper.Map<SiteUserEditDto, SiteUser>(dbModel);
					contextFactory.Component.SiteUsers.Add(user);
				}

				if(user.Roles == null)
					user.Roles = new List<SiteRole>();

				user.Roles.Add(contextFactory.Component.SiteRoles.FirstOrDefault(r => r.Id == dbModel.RoleId));
				contextFactory.Component.SaveChanges();
			}
		}

		public void RemoveUserFromRole(int userId, int roleId) {
			using(var contextFactory = ComponentContainer.Component<PermissionsContext>()) {
				var user =
				contextFactory.Component.SiteUsers.Include(u => u.Roles)
					.FirstOrDefault(u => u.Id == userId && u.Roles.Any(r => r.Id == roleId));
				if(user != null) {
					user.Roles.Remove(user.Roles.First(r => r.Id == roleId));
					contextFactory.Component.Entry(user).State = EntityState.Modified;
					contextFactory.Component.SaveChanges();
				}
			}
		}

		public TResult GetUserBySid<TResult>(string sid) {
			using(var contextFactory = ComponentContainer.Component<PermissionsContext>()) {
				var user = contextFactory.Component.SiteUsers.FirstOrDefault(obj => obj.UserId == sid);
				return user != null ? Mapper.Map<SiteUser, TResult>(user) : default(TResult);
			}
		}

		public bool IsUserInRole(string sid, string roleName) {
			using(var contextFactory = ComponentContainer.Component<PermissionsContext>()) {
				return contextFactory.Component.SiteUsers.Any(u => u.UserId == sid && u.Roles.Any(r => r.Name == roleName));
			}
		}

		public IEnumerable<SiteRoleIndexDto> GetRolesForUser(string sid) {
			using(var contextFactory = ComponentContainer.Component<PermissionsContext>()) {
				var query = contextFactory.Component.SiteUsers.Include(u => u.Roles).FirstOrDefault(u => u.UserId == sid);

				if(query == null)
					yield break;
				foreach(var role in query.Roles)
					yield return Mapper.Map<SiteRole, SiteRoleIndexDto>(role);
			}
		}

		public IEnumerable<SiteGroupIndexDto> GetGroupsForUser(string sid) {
			using(var contextFactory = ComponentContainer.Component<PermissionsContext>()) {
				var query = contextFactory.Component.SiteUsers.Include(u => u.Groups).FirstOrDefault(u => u.UserId == sid);

				if(query == null)
					yield break;
				foreach(var role in query.Groups)
					yield return Mapper.Map<SiteGroup, SiteGroupIndexDto>(role);
			}
		}
	}
}