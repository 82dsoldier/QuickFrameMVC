using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGeneration.DotNet;
using QuickFrame.Interfaces;
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
			var libraryManager = services.FirstOrDefault(s => s.ServiceType == typeof(ILibraryManager)).ImplementationInstance as ILibraryManager;

			List<IFileProvider> providerList = GetFileProviderList(libraryManager).ToList();

			services.Configure<RazorViewEngineOptions>(options => {
				foreach(var provider in providerList)
					options.FileProviders.Add(provider);
			});

			return services;
		}

		public static IApplicationBuilder UseEmbeddedFileProviders(this IApplicationBuilder app, ILibraryManager libraryManager) {
			List<IFileProvider> providerList = GetFileProviderList(libraryManager).ToList();

			IOptions<RazorViewEngineOptions> razorViewEngineOptions =
				app.ApplicationServices.GetService<IOptions<RazorViewEngineOptions>>();

			foreach(var provider in providerList)
				razorViewEngineOptions.Value.FileProviders.Add(provider);

			return app;
		}

		private static IEnumerable<IFileProvider> GetFileProviderList(ILibraryManager libraryManager) {
#if NETCOREAPP1_0
			foreach (var library in libraryManager.GetLibraries()) {
				Assembly assembly = null;
				try {
					assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(library.Path);
				} catch {
					continue;
				}

				if (assembly != null) {
					var providerContainer = assembly.GetTypes().FirstOrDefault(t => typeof(IEmbeddedFileProviderContainer).IsAssignableFrom(t) && !t.GetTypeInfo().IsInterface);
					if (providerContainer != null) {
						var container = Activator.CreateInstance(providerContainer);
						yield return (container as IEmbeddedFileProviderContainer).FileProvider;
					}
				}
			}
#else
			foreach(var assemblyName in Assembly.GetAssembly(typeof(EmbeddedFileProviderExtensions)).GetReferencedAssemblies()) {
				var assembly = Assembly.Load(assemblyName);
				var providerContainer = assembly.GetTypes().FirstOrDefault(t => typeof(IEmbeddedFileProviderContainer).IsAssignableFrom(t) && !t.GetTypeInfo().IsInterface);
				if(providerContainer != null) {
					var container = Activator.CreateInstance(providerContainer);
					yield return (container as IEmbeddedFileProviderContainer).FileProvider;
				}
			}
#endif
		}
	}
}