using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QuickFrame.Security.AccountControl.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace QuickFrame.Security.AccountControl {

	public class QuickFrameUserManager : UserManager<SiteUser> {
		private IUserStore<SiteUser> _userStore;

		public QuickFrameUserManager(IUserStore<SiteUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<SiteUser> passwordHasher,
			IEnumerable<IUserValidator<SiteUser>> userValidators, IEnumerable<IPasswordValidator<SiteUser>> passwordValidators, ILookupNormalizer keyNormalizer,
			IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<SiteUser>> logger)
			: base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger) {
			_userStore = store;
		}

		public override string GetUserId(ClaimsPrincipal principal) {
			return principal.FindFirstValue(ClaimTypes.PrimarySid);
		}
	}
}