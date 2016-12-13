using System.Linq;
using System.Security.Claims;

namespace QuickFrame.Security {

	public static class Extensions {

		public static string GetSid(this ClaimsPrincipal user)
			=> user.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.PrimarySid.ToString())?.Value;
	}
}
