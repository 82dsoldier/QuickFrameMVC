using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuickFrame.Security.AccountControl.Interfaces.Services;
using QuickFrame.Security.AccountControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace QuickFrame.Security {

	public class QuickFrameSecurityManager {
		private UserManager<SiteUser> _userManager;
		private ISiteRulesDataService _siteRulesDataService;

		public QuickFrameSecurityManager(UserManager<SiteUser> userManager, ISiteRulesDataService siteRulesDataService) {
			_userManager = userManager;
			_siteRulesDataService = siteRulesDataService;
		}

		public IActionResult AuthorizeExecution(ClaimsPrincipal user, string currentUrl, Func<IActionResult> func) {
			if(ExecuteWithoutSecurity())
				return func();

			var claim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Authentication);

			if(claim == null)
				GetRolesForPrincipal(user);

			foreach(var id in user.Identities) {
				var ruleList = GetSiteRulesForIdentity(id).Where(r => IsUrlMatch(r.Url, currentUrl) != false).OrderBy(r => r.Priority);

				foreach(var rule in ruleList) {
					if(IsUrlMatch(rule.Url, currentUrl) == true) {
						if(rule.IsAllow) {
							id.AddClaim(new Claim(ClaimTypes.Uri, currentUrl));
							return func();
						}
						return (new UnauthorizedResult());
					} else if(IsUrlMatch(rule.Url, currentUrl) == null) {
						if(rule.MatchPartial)
							return rule.IsAllow ? func() : new UnauthorizedResult();
						return (new UnauthorizedResult());
					}
				}
			}
			return (new UnauthorizedResult());
		}

		public void GetRolesForPrincipal(ClaimsPrincipal principal) {
			var claim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
			if(claim == null) {
				var id = (principal.Identity as ClaimsIdentity).Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
				if(id != null)
					(principal.Identity as ClaimsIdentity).AddClaim(new Claim(ClaimTypes.NameIdentifier, id));
			}

			var task = _userManager.GetUserAsync(principal);
			task.Wait();

			var user = task.Result;
			var roleTask = _userManager.GetRolesAsync(user);
			roleTask.Wait();
			var roleList = roleTask.Result;

			foreach(var role in roleList)
				(principal.Identity as ClaimsIdentity).AddClaim(new Claim(ClaimTypes.Role, role));

			(principal.Identity as ClaimsIdentity).AddClaim(new Claim(ClaimTypes.Authentication, "true"));
		}

		private bool ExecuteWithoutSecurity() {
			var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
			var sec = Environment.GetEnvironmentVariable("NOSECURITY");
			var useSecurity = true;

			if(env != null) {
				if(env.Equals("DEBUG", StringComparison.CurrentCultureIgnoreCase)) {
					Boolean.TryParse(sec, out useSecurity);
				}
			}

			return !useSecurity;
		}

		private IEnumerable<SiteRule> GetSiteRulesForIdentity(ClaimsIdentity id) {
			foreach(var rule in _siteRulesDataService.GetSiteRulesForUser(id.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value))
				yield return rule;
			foreach(var claim in id.Claims.Where(c => c.Type == ClaimTypes.GroupSid || c.Type == ClaimTypes.PrimaryGroupSid))
				foreach(var rule in _siteRulesDataService.GetSiteRulesForGroup(claim.Value))
					yield return rule;
			foreach(var claim in id.Claims.Where(c => c.Type == ClaimTypes.Role))
				foreach(var rule in _siteRulesDataService.GetSiteRulesForRole(claim.Value))
					yield return rule;
		}

		private bool? IsUrlMatch(string first, string second) {
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
	}
}