using Microsoft.AspNet.Builder;
using Microsoft.AspNet.FileProviders;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Extensions.PlatformAbstractions;
using QuickFrame.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QuickFrame.Mvc {

	public static class EmbeddedFileProviderExtensions {

		public static IApplicationBuilder UseEmbeddedFileProviders(this IApplicationBuilder app, ILibraryManager libraryManager, IAssemblyLoadContextAccessor assemblyLoadContextAccessor) {
			IOptions<RazorViewEngineOptions> razorViewEngineOptions =
				app.ApplicationServices.GetService<IOptions<RazorViewEngineOptions>>();

			List<IFileProvider> providerList = new List<IFileProvider>() {
				razorViewEngineOptions.Value.FileProvider
			};

			var loadContext = assemblyLoadContextAccessor.Default;

			foreach (var library in libraryManager.GetLibraries()) {
				Assembly assembly = null;
				try {
					assembly = loadContext.Load(library.Name);
				} catch {
					continue;
				}

				if (assembly != null) {
					var providerContainer = assembly.GetTypes().FirstOrDefault(t => typeof(IEmbeddedFileProviderContainer).IsAssignableFrom(t) && !t.IsInterface);
					if (providerContainer != null) {
						var container = Activator.CreateInstance(providerContainer);
						providerList.Add((container as IEmbeddedFileProviderContainer).FileProvider);
					}
				}
			}

			razorViewEngineOptions.Value.FileProvider = new CompositeFileProvider(providerList);

			return app;
		}
	}
}