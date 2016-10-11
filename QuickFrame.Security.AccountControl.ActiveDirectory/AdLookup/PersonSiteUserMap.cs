using ExpressMapper;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using QuickFrame.Security.AccountControl.Models;

namespace QuickFrame.Security.AccountControl.ActiveDirectory.AdLookup {

	public class PersonSiteUserMap : ICustomTypeMapper<Person, SiteUser> {

		public SiteUser Map(IMappingContext<Person, SiteUser> context) {
			var retVal = new SiteUser {
				DisplayName = context.Source.DisplayName,
				Id = context.Source.Id,
				NormalizedUserName = context.Source.UserName.ToUpper(),
				Email = context.Source.Email.Count > 0 ? context.Source.Email[0] : null,
				NormalizedEmail = context.Source.Email.Count > 0 ? context.Source.Email[0].ToUpper() : null,
				PhoneNumber = context.Source.PhoneNumber.Count > 0 ? context.Source.PhoneNumber[0] : null,
				UserName = context.Source.UserName,
				FirstName = context.Source.FirstName,
				LastName = context.Source.LastName
			};

			foreach(var obj in context.Source.Claims) {
				retVal.Claims.Add(new IdentityUserClaim<string> {
					ClaimType = obj.Key,
					ClaimValue = obj.Value
				});
			}

			return retVal;
		}
	}
}