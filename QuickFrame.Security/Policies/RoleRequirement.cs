using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QuickFrame.Security.Policies {

	public class RoleRequirement : AuthorizationHandler<OperationAuthorizationRequirement, string> {
		private QuickFrameSecurityManager _securityManager;

		public RoleRequirement(QuickFrameSecurityManager securityManager) {
			_securityManager = securityManager;
		}

		public static OperationAuthorizationRequirement IsMemberOf = new OperationAuthorizationRequirement {
			Name = "IsMemberOf"
		};

		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, string resource) {
			var claim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Authentication);
			if(claim == null)
				_securityManager.GetRolesForPrincipal(context.User);
			if(context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role && c.Value == resource) == null)
				context.Fail();
			else
				context.Succeed(requirement);
			return Task.FromResult(true);
		}
	}
}