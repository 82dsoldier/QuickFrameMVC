using ExpressMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using QuickFrame.Data;
using QuickFrame.Security.AccountControl;
using QuickFrame.Security.AccountControl.Data;
using QuickFrame.Security.AccountControl.Interfaces.Services;
using QuickFrame.Security.AccountControl.Models;
using QuickFrame.Security.AccountControl.Services;
using QuickFrame.Security.Configuration;
using QuickFrame.Security.Data;
using QuickFrame.Security.Data.Dtos;
using QuickFrame.Security.Data.Events;
using QuickFrame.Security.Data.Models;
using QuickFrame.Security.Policies;
using System.Reflection;

namespace QuickFrame.Security {

	public static class ServiceExtensions {

		public static IServiceCollection AddQuickFrameSecurity(this IServiceCollection services, IConfigurationRoot configuration) {
			services.AddIdentity<SiteUser, SiteRole>()
				.AddUserManager<QuickFrameUserManager>();

			services.AddSingleton<TrackingContext>()
				.AddExpressMapperObjects(typeof(TrackingContext).GetTypeInfo().Assembly)
				.Configure<RazorViewEngineOptions>(options => {
					options.FileProviders.Add(
						new CompositeFileProvider(
							new EmbeddedFileProvider(typeof(TrackingContext).GetTypeInfo().Assembly, "QuickFrame.Security")));
				})
				.AddTransient<IAuthorizationHandler, RoleRequirement>()
				.AddTransient<QuickFrameSecurityManager>()
				.AddTransient<ISiteRulesDataService, SiteRulesDataService>()
				.AddTransient<GroupManager<SiteGroup>>()
				.AddTransient<QuickFrameIdentityErrorDescriber>()
				.AddScoped<SecurityContext>();

			Mapper.Register<AuditLog, DataChangedEventArgs>()
				.Function(dest => dest.EventType, src => {
					return (EntityState)src.EventType;
				});

			Mapper.Register<DataChangedEventArgs, DataChangedEventArgsDto>();

			services.Configure<NameListOptions>(excluded => {
				excluded.Load(configuration.GetSection("ExcludedNames"));
			});

			return services;
		}
	}
}