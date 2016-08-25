using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using QuickFrame.Configuration;
using QuickFrame.Di;
using QuickFrame.Mvc;
using QuickFrame.Mvc.Configuration;
using System.Reflection;

namespace QuickFrame.Mvc {

	public static class ServiceExtensions {
		private static ContainerBuilder _builder => ComponentContainer.Builder;

		public static IServiceCollection AddQuickFrameMvc(this IServiceCollection services, IConfigurationRoot configuration) {
			services.AddTransient<IActionContextAccessor, ActionContextAccessor>();

			services.Configure<ViewOptions>(viewOptions => {
				viewOptions.PerPageDefault = configuration["ViewOptions:PerPageDefault"];

				foreach(var child in configuration.GetSection("ViewOptions:PerPageList").GetChildren()) {
					viewOptions.PerPageList.Add(new SelectListItem {
						Value = child.Key,
						Text = child.Value,
						Selected = (viewOptions.PerPageDefault == child.Value)
					});
				}
			});

			services.Configure<QuickFrameMvcOptions>(opts => {
				foreach(var child in configuration.GetSection("QuickFrameMvcOptions:ExplorerBasePaths").GetChildren()) {
					opts.ExplorerBasePaths.Add(child.Value);
				}
			});

			services.Configure<RazorViewEngineOptions>(options => {
				options.FileProviders.Add(new EmbeddedFileProvider(
					typeof(FluentTagBuilder).GetTypeInfo().Assembly,
					"QuickFrame.Mvc"));
			});
			return services;
		}

		public static IApplicationBuilder UseQuickFrameMvc(this IApplicationBuilder app) {
			IOptions<RazorViewEngineOptions> razorViewEngineOptions =
				app.ApplicationServices.GetService<IOptions<RazorViewEngineOptions>>();
			razorViewEngineOptions.Value.FileProviders.Add(new EmbeddedFileProvider(
					typeof(FluentTagBuilder).GetTypeInfo().Assembly,
					"QuickFrame.Mvc"));
			return app;
		}
	}
}