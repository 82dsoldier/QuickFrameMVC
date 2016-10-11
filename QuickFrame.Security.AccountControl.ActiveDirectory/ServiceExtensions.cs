using ExpressMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using QuickFrame.Security.AccountControl.ActiveDirectory.AdLookup;
using QuickFrame.Security.AccountControl.Interfaces;
using QuickFrame.Security.AccountControl.Models;

namespace QuickFrame.Security.AccountControl.ActiveDirectory {

	public static class ServiceExtensions {

		public static IServiceCollection AddQuickFrameSecurityAd(this IServiceCollection services) {
			services.AddTransient<IUserStore<SiteUser>, AdUserStore>()
				.AddTransient<IGroupStore<SiteGroup>, AdGroupStore>();

			Mapper.RegisterCustom<Person, SiteUser, PersonSiteUserMap>();

			return services;
		}
	}
}