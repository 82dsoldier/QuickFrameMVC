using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using QuickFrame.Mvc.Configuration;
using QuickFrame.Mvc.ExceptionHandling;
using System.Reflection;

namespace QuickFrame.Mvc {

	public static class ServiceExtensions {

		public static IServiceCollection AddQuickFrameMvc(this IServiceCollection services, IConfigurationRoot configuration) {
			services.Configure<RazorViewEngineOptions>(options => {
				options.FileProviders.Add(new CompositeFileProvider(new EmbeddedFileProvider(typeof(ServiceOptions).GetTypeInfo().Assembly, "QuickFrame.Mvc")));
			});

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

			return services;
		}

		public static IServiceCollection AddQuickFrameExceptionHandling(this IServiceCollection services) {
			return services.Configure<MvcOptions>(options => {
				options.Filters.Add(new QuickFrameExceptionFilter());
			});
		}
	}
}