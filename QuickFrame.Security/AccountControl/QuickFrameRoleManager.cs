using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using QuickFrame.Security.AccountControl.Data.Models;
using System.Collections.Generic;

namespace QuickFrame.Security.AccountControl {

	public class QuickFrameRoleManager : RoleManager<SiteRole> {

		public QuickFrameRoleManager(IRoleStore<SiteRole> store, IEnumerable<IRoleValidator<SiteRole>> roleValidators, ILookupNormalizer keyNormalizer,
			IdentityErrorDescriber errors, ILogger<RoleManager<SiteRole>> logger, IHttpContextAccessor contextAccessor)
			: base(store, roleValidators, keyNormalizer, errors, logger, contextAccessor) {
		}
	}
}