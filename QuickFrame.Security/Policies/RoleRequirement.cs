using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Authorization.Infrastructure;
using QuickFrame.Security.Data.Interfaces;
using System.ComponentModel.Composition;
using System.Linq;
using System.Security.Claims;

namespace QuickFrame.Security.Policies {

	[Export(typeof(IAuthorizationHandler))]
	public class RoleRequirement : AuthorizationHandler<OperationAuthorizationRequirement, string> {
		private readonly ISiteUsersDataService _siteUsersData;
		private readonly ISiteGroupsDataService _siteGroupsData;

		public RoleRequirement(ISiteUsersDataService siteUsersData, ISiteGroupsDataService siteGroupsData) {
			_siteUsersData = siteUsersData;
			_siteGroupsData = siteGroupsData;
		}

		public static OperationAuthorizationRequirement IsMemberOf = new OperationAuthorizationRequirement {
			Name = "IsMemberOf"
		};

		protected override void Handle(AuthorizationContext context, OperationAuthorizationRequirement requirement, string resource) {
			//First check to see if the user is directly in the role
			if (
				_siteUsersData.IsUserInRole(
					context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid.ToString())?.Value, resource)) {
				context.Succeed(requirement);
				return;
			}

			//If not, check groups that may belong to the role.

			if (context.User.Claims.Where(
				c => c.Type == ClaimTypes.GroupSid.ToString() || c.Type == ClaimTypes.PrimaryGroupSid.ToString()).Any(groupName => _siteGroupsData.IsGroupInRole(groupName.Value, resource))) {
				context.Succeed(requirement);
				return;
			}
			context.Fail();
		}
	}
}