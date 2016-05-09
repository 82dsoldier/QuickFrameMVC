﻿using Microsoft.AspNet.Mvc;
using QuickFrame.Di;
using QuickFrame.Security.Attributes;
using QuickFrame.Security.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QuickFrame.Security
{
    public static class AuthorizationExtensions
    {
		public static IActionResult AuthorizeExecution(ClaimsPrincipal User, Func<IActionResult> func, [CallerMemberName]string callerName = "") {
#if DEBUG
			return func();
#else

			var deniedList = new List<string>();
			var allowedList = new List<string>();

			var callingMethod = new StackTrace().GetFrames().FirstOrDefault(f => f.GetMethod().Name == callerName)?.GetMethod();

			if (callingMethod != null) {
				var methodRoles = callingMethod.GetCustomAttribute<RolesAttribute>();

				if (methodRoles != null) {
					if (methodRoles.DeniedRoles != null)
						deniedList.AddRange(methodRoles.DeniedRoles);
					if (methodRoles.AllowedRoles != null) {
						allowedList.AddRange(methodRoles.AllowedRoles);
						foreach (var role in methodRoles.AllowedRoles.Where(role => deniedList.Contains(role)))
							deniedList.Remove(role);
					}
				}

				var callingClass = callingMethod.DeclaringType;

				if (callingClass != null) {
					var classRoles = callingClass.GetCustomAttribute<RolesAttribute>();

					if (classRoles.DeniedRoles != null)
						deniedList.AddRange(classRoles.DeniedRoles);
					if (classRoles.AllowedRoles != null)
						allowedList.AddRange(classRoles.AllowedRoles);
				}
			}

			var roleList = new List<string>();

			using (var siteUsers = ComponentContainer.Component<ISiteUsersDataService>()) {
				using (var siteGroups = ComponentContainer.Component<ISiteGroupsDataService>()) {
					roleList.AddRange(siteUsers.Component.GetRolesForUser(User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.PrimarySid.ToString())?.Value).Select(role => role.Name));
					roleList.AddRange(User.Claims.Where(claim => claim.Type == ClaimTypes.GroupSid.ToString() || claim.Type == ClaimTypes.PrimaryGroupSid.ToString()).SelectMany(@group => siteGroups.Component.GetRolesForGroup(@group.Value)).Select(role => role.Name));
				}
			}

			if (deniedList.Any(role => roleList.Any(r => r == role)))
				return new HttpUnauthorizedResult();

			if (!allowedList.Any(role => roleList.Any(r => r == role)))
				return new HttpUnauthorizedResult();

			return func();
#endif
		}
	}
}
