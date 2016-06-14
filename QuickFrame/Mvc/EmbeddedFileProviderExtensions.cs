using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using QuickFrame.Interfaces;
using QuickFrame.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#if NETCOREAPP1_0
using System.Runtime.Loader;
#endif

namespace QuickFrame.Mvc {

	public static class EmbeddedFileProviderExtensions {

		public static IServiceCollection AddEmbeddedFileProviders(this IServiceCollection services) {
			services.Configure<RazorViewEngineOptions>(options => {
				foreach(var provider in GetFileProviderList())
					options.FileProviders.Add(provider);
			});

			return services;
		}

		public static IApplicationBuilder UseEmbeddedFileProviders(this IApplicationBuilder app) {

			IOptions<RazorViewEngineOptions> razorViewEngineOptions =
				app.ApplicationServices.GetService<IOptions<RazorViewEngineOptions>>();

			foreach(var provider in GetFileProviderList())
				razorViewEngineOptions.Value.FileProviders.Add(provider);

			return app;
		}

		private static IEnumerable<IFileProvider> GetFileProviderList() {

			foreach(var assembly in IO.GetAssemblies()) {

				if(assembly != null) {
					Type providerContainer = null;
					try {
						providerContainer = assembly.GetTypes().FirstOrDefault(t => typeof(IEmbeddedFileProviderContainer).IsAssignableFrom(t) && !t.GetTypeInfo().IsInterface);
					} catch {
					}
					if(providerContainer != null) {
						var container = Activator.CreateInstance(providerContainer);
						yield return (container as IEmbeddedFileProviderContainer).FileProvider;
					}
				}
			}

		}
	}
}