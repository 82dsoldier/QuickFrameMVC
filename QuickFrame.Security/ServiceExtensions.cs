﻿using Autofac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using QuickFrame.Data.Services;
using QuickFrame.Di;
using QuickFrame.Security.AccountControl;
using QuickFrame.Security.AccountControl.Data;
using QuickFrame.Security.AccountControl.Data.Models;
using QuickFrame.Security.Policies;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;

namespace QuickFrame.Security {

	public static class ServiceExtensions {
		private static ContainerBuilder _builder => ComponentContainer.Builder;

		public static IServiceCollection AddQuickFrameSecurity(this IServiceCollection services) {
			_builder.RegisterType(typeof(SecurityContext)).InstancePerLifetimeScope();
			foreach(var obj in typeof(SecurityContext).Assembly.GetTypes().Where(t => t.GetTypeInfo().GetCustomAttribute<ExportAttribute>() != null || typeof(DataService<,,>).IsAssignableFrom(t))) {
				ExportAttribute att = obj.GetCustomAttribute<ExportAttribute>();
				if(att?.ContractType != null) {
					_builder.RegisterType(obj).As(att.ContractType);
				} else {
					foreach(var intf in obj.GetInterfaces())
						_builder.RegisterType(obj).As(intf);
				}
			}
			_builder.RegisterType(typeof(QuickFrameIdentityErrorDescriber));
			_builder.RegisterType(typeof(GroupManager<SiteGroup>));
			_builder.RegisterType(typeof(QuickFrameRoleStore)).As<IRoleStore<SiteRole>>();
			services.Configure<RazorViewEngineOptions>(options => {
				options.FileProviders.Add(new EmbeddedFileProvider(
					typeof(EmbeddedFileProviderContainer).GetTypeInfo().Assembly,
					"QuickFrame.Security"));
			});
			services.AddTransient<IAuthorizationHandler, RoleRequirement>();
			services.AddIdentity<SiteUser, SiteRole>()
				.AddUserManager<QuickFrameUserManager>()
				.AddRoleManager<QuickFrameRoleManager>();
			return services;
		}

		public static IApplicationBuilder UseQuickFrameSecurity(this IApplicationBuilder app) {
			IOptions<RazorViewEngineOptions> razorViewEngineOptions =
				app.ApplicationServices.GetService<IOptions<RazorViewEngineOptions>>();
			razorViewEngineOptions.Value.FileProviders.Add(new EmbeddedFileProvider(
					typeof(EmbeddedFileProviderContainer).GetTypeInfo().Assembly,
					"QuickFrame.Security"));

			var userManager = ComponentContainer.Component<QuickFrameUserManager>();
			var roleManager = ComponentContainer.Component<QuickFrameRoleManager>();

			app.UseClaimsTransformation(new ClaimsTransformationOptions {
				Transformer = new QuickFrameClaimsTransformer(userManager, roleManager)
			});

			return app;
		}
	}
}