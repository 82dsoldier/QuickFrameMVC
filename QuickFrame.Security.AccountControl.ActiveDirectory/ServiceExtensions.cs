using Autofac;
using ExpressMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using QuickFrame.Data.Services;
using QuickFrame.Di;
using QuickFrame.Security.AccountControl.ActiveDirectory.AdLookup;
using QuickFrame.Security.AccountControl.ActiveDirectory.Configuration;
using QuickFrame.Security.AccountControl.Data.Models;
using QuickFrame.Security.AccountControl.Interfaces;
using QuickFrame.Security.Configuration;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;

namespace QuickFrame.Security.AccountControl.ActiveDirectory {

	public static class ServiceExtensions {
		private static ContainerBuilder _builder => ComponentContainer.Builder;

		public static IServiceCollection AddQuickFrameSecurityAd(this IServiceCollection services, IConfigurationRoot configuration) {
			foreach(var obj in typeof(AdUserStore).Assembly.GetTypes().Where(t => t.GetTypeInfo().GetCustomAttribute<ExportAttribute>() != null || typeof(DataService<,,>).IsAssignableFrom(t))) {
				ExportAttribute att = obj.GetCustomAttribute<ExportAttribute>();
				if(att?.ContractType != null) {
					_builder.RegisterType(obj).As(att.ContractType);
				} else {
					foreach(var intf in obj.GetInterfaces())
						_builder.RegisterType(obj).As(intf);
				}
			}
			_builder.RegisterType(typeof(AdUserStore)).As<IUserStore<SiteUser>>();
			_builder.RegisterType(typeof(AdGroupStore)).As<IGroupStore<SiteGroup>>();
			Mapper.RegisterCustom<Person, SiteUser, PersonSiteUserMap>();
			services.Configure<RazorViewEngineOptions>(options => {
				options.FileProviders.Add(new EmbeddedFileProvider(
					typeof(EmbeddedFileProviderContainer).GetTypeInfo().Assembly,
					"QuickFrame.Security.AccountControl.ActiveDirectory"));
			});

			services.Configure<NameListOptions>(excluded => {
				excluded.Load(configuration.GetSection("ExcludedNames"));
			});

			services.Configure<AdOptions>(opts => {
				opts.SearchPath = configuration["AdOptions:SearchPath"];
			});

			return services;
		}

		public static IApplicationBuilder UseQuickFrameSecurityAd(this IApplicationBuilder app) {
			IOptions<RazorViewEngineOptions> razorViewEngineOptions =
				app.ApplicationServices.GetService<IOptions<RazorViewEngineOptions>>();
			razorViewEngineOptions.Value.FileProviders.Add(new EmbeddedFileProvider(
					typeof(EmbeddedFileProviderContainer).GetTypeInfo().Assembly,
					"QuickFrame.Security.AccountControl.ActiveDirectory"));
			return app;
		}
	}
}