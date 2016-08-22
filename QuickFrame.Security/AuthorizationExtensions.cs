using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuickFrame.Di;
using QuickFrame.Security.AccountControl.Data.Models;
using QuickFrame.Security.AccountControl.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace QuickFrame.Security {

	public static class AuthorizationExtensions {

		public static IActionResult AuthorizeExecution(ClaimsPrincipal user, string currentUrl, Func<IActionResult> func) {
			var claim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Authentication);
			if(claim == null)
				GetRolesForPrincipal(user);

			foreach(var id in user.Identities) {
				var ruleList = GetSiteRulesForIdentity(id).Where(r => r.Url.IsUrlMatch(currentUrl) != false).OrderBy(r => r.Priority);

				foreach(var rule in ruleList) {
					if(rule.Url.IsUrlMatch(currentUrl) == true) {
						if(rule.IsAllow) {
							id.AddClaim(new Claim(ClaimTypes.Uri, currentUrl));
							return func();
						}
						return (IActionResult)(new UnauthorizedResult());
					} else if(rule.Url.IsUrlMatch(currentUrl) == null) {
						if(rule.MatchPartial)
							return rule.IsAllow ? func() : new UnauthorizedResult();
						return (IActionResult)(new UnauthorizedResult());
					}
				}
			}
			return (IActionResult)(new UnauthorizedResult());
		}

		public static bool? IsUrlMatch(this string first, string second) {
			var currentUri = new Uri(second);
			var baseUri = new Uri(String.Format("{0}{1}", $"{currentUri.Scheme}://{currentUri.Host}", currentUri.Port != 0 ? $":{currentUri.Port}" : String.Empty));

			var ruleUri = default(Uri);
			try {
				ruleUri = new Uri(first);
			} catch {
				ruleUri = new Uri(baseUri, new Uri(first, UriKind.Relative));
			}

			var currentString = currentUri.PathAndQuery;
			var ruleString = ruleUri.PathAndQuery;

			if(currentString.StartsWith(ruleString, StringComparison.CurrentCultureIgnoreCase))
				if(currentString.Equals(ruleString, StringComparison.CurrentCultureIgnoreCase))
					return true;
				else
					return null;
			return false;
		}

		public static void GetRolesForPrincipal(ClaimsPrincipal principal) {
			var claim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
			if(claim == null) {
				var id = (principal.Identity as ClaimsIdentity).Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
				if(id != null)
					(principal.Identity as ClaimsIdentity).AddClaim(new Claim(ClaimTypes.NameIdentifier, id));
			}

			using(var userManager = ComponentContainer.Component<UserManager<SiteUser>>()) {
				var task = userManager.Component.GetUserAsync(principal);
				task.Wait();
				var user = task.Result;
				var roleTask = userManager.Component.GetRolesAsync(user);
				roleTask.Wait();
				var roleList = roleTask.Result;
				foreach(var role in roleList)
					(principal.Identity as ClaimsIdentity).AddClaim(new Claim(ClaimTypes.Role, role));
			}

			(principal.Identity as ClaimsIdentity).AddClaim(new Claim(ClaimTypes.Authentication, "true"));
		}

		private static IEnumerable<SiteRule> GetSiteRulesForIdentity(ClaimsIdentity id) {
			using(var service = ComponentContainer.Component<ISiteRulesDataService>()) {
				foreach(var rule in service.Component.GetSiteRulesForUser(id.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value))
					yield return rule;
				foreach(var claim in id.Claims.Where(c => c.Type == ClaimTypes.GroupSid || c.Type == ClaimTypes.PrimaryGroupSid))
					foreach(var rule in service.Component.GetSiteRulesForGroup(claim.Value))
						yield return rule;
				foreach(var claim in id.Claims.Where(c => c.Type == ClaimTypes.Role))
					foreach(var rule in service.Component.GetSiteRulesForRole(claim.Value))
						yield return rule;
			}
		}
	}
}

