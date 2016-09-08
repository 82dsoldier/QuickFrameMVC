using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using static QuickFrame.Security.AuthorizationExtensions;

namespace QuickFrame.Security.AccountControl {

	public class QuickFrameClaimsTransformer : IClaimsTransformer {
		private QuickFrameUserManager _userManager;
		private QuickFrameRoleManager _roleManager;

		public Task<ClaimsPrincipal> TransformAsync(ClaimsTransformationContext context) {
			if(!ExecuteWithoutSecurity())
				GetRolesForPrincipal(context.Principal);

			return Task.FromResult(context.Principal);
			//var claim = context.Principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
			//if(claim == null) {
			//	var id = (context.Principal.Identity as ClaimsIdentity).Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
			//	if(id != null)
			//		(context.Principal.Identity as ClaimsIdentity).AddClaim(new Claim(ClaimTypes.NameIdentifier, id));

			//}

			//var user = await _userManager.GetUserAsync(context.Principal);
			//var roleList = await _userManager.GetRolesAsync(user);
			//foreach(var role in roleList)
			//	(context.Principal.Identity as ClaimsIdentity).AddClaim(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", role));
		}

		public QuickFrameClaimsTransformer(QuickFrameUserManager userManager, QuickFrameRoleManager roleManager) {
			_userManager = userManager;
			_roleManager = roleManager;
		}
	}
}