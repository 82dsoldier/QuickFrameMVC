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

		public static IServiceCollection AddEmbeddedFileProviders(this IServiceCollection services) {
			var assemblyLoadContextAccessor = services.FirstOrDefault(s => s.ServiceType == typeof(IAssemblyLoadContextAccessor)).ImplementationInstance as IAssemblyLoadContextAccessor;
			var libraryManager = services.FirstOrDefault(s => s.ServiceType == typeof(ILibraryManager)).ImplementationInstance as ILibraryManager;

			List<IFileProvider> providerList = GetFileProviderList(assemblyLoadContextAccessor, libraryManager).ToList();

			services.Configure<RazorViewEngineOptions>(options => {
				providerList.Add(options.FileProvider);
				options.FileProvider = new CompositeFileProvider(providerList.ToArray());
			});

			return services;
		}

		public static IApplicationBuilder UseEmbeddedFileProviders(this IApplicationBuilder app, IAssemblyLoadContextAccessor assemblyLoadContextAccessor, ILibraryManager libraryManager) {

			List<IFileProvider> providerList = GetFileProviderList(assemblyLoadContextAccessor, libraryManager).ToList();

			IOptions<RazorViewEngineOptions> razorViewEngineOptions =
				app.ApplicationServices.GetService<IOptions<RazorViewEngineOptions>>();

			providerList.Add(razorViewEngineOptions.Value.FileProvider);

			razorViewEngineOptions.Value.FileProvider = new CompositeFileProvider(providerList);

			return app;
		}

		private static IEnumerable<IFileProvider> GetFileProviderList(IAssemblyLoadContextAccessor assemblyLoadContextAccessor, ILibraryManager libraryManager) {

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
						yield return (container as IEmbeddedFileProviderContainer).FileProvider;
					}
				}
			}
		}
	}
}