using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Composition;
using System.Threading.Tasks;

namespace QuickFrame.Security.Policies {

	[Export(typeof(IAuthorizationHandler))]
	public class RoleRequirement : AuthorizationHandler<OperationAuthorizationRequirement, string> {
		//private readonly ISiteUsersDataService _siteUsersData;
		//private readonly ISiteGroupsDataService _siteGroupsData;

		//public RoleRequirement(/*ISiteUsersDataService siteUsersData,*/ ISiteGroupsDataService siteGroupsData) {
		//	//_siteUsersData = siteUsersData;
		//	_siteGroupsData = siteGroupsData;
		//}

		public static OperationAuthorizationRequirement IsMemberOf = new OperationAuthorizationRequirement {
			Name = "IsMemberOf"
		};

		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, string resource) {
			return Task.Run(() => {
				context.Fail();
			});
			//First check to see if the user is directly in the role
			//if(
			//	_siteUsersData.IsUserInRole(
			//		context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid.ToString())?.Value, resource)) {
			//	context.Succeed(requirement);
			//	return;
			//}

			//If not, check groups that may belong to the role.

			//if(context.User.Claims.Where(
			//	c => c.Type == ClaimTypes.GroupSid.ToString() || c.Type == ClaimTypes.PrimaryGroupSid.ToString()).Any(groupName => _siteGroupsData.IsGroupInRole(groupName.Value, resource))) {
			//	context.Succeed(requirement);
			//	return;
			//}
		}
	}
}