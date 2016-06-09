using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
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
			List<IFileProvider> providerList = GetFileProviderList().ToList();

			services.Configure<RazorViewEngineOptions>(options => {
				foreach(var provider in providerList)
					options.FileProviders.Add(provider);
			});

			return services;
		}

		public static IApplicationBuilder UseEmbeddedFileProviders(this IApplicationBuilder app) {
			List<IFileProvider> providerList = GetFileProviderList().ToList();

			IOptions<RazorViewEngineOptions> razorViewEngineOptions =
				app.ApplicationServices.GetService<IOptions<RazorViewEngineOptions>>();

			foreach(var provider in providerList)
				razorViewEngineOptions.Value.FileProviders.Add(provider);

			return app;
		}

		private static IEnumerable<IFileProvider> GetFileProviderList() {
#if NETCOREAPP1_0
			//This code should fix this when I get around to it:
			/*
			var loadableAssemblies = new List<Assembly>();

var deps = DependencyContext.Default;
foreach (var compilationLibrary in deps.CompileLibraries)
{
    if (compilationLibrary.Name.Contains(projectNamespace))
    {
        var assembly = Assembly.Load(new AssemblyName(compilationLibrary.Name));
        loadableAssemblies.Add(assembly);
    }
}*/

			//This code no longer works
/*			foreach (var library in libraryManager.GetLibraries()) {
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
			}*/
			return null;
#else
			var deps = DependencyContext.Default;
			foreach(var compilationLibrary in deps.CompileLibraries) {
				//if(compilationLibrary.Name.Contains(projectNamespace)) {
				Assembly assembly = null;
				try {
					assembly = Assembly.Load(new AssemblyName(compilationLibrary.Name));
				} catch {
				}
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

			//foreach(var assemblyName in Assembly.GetAssembly(typeof(EmbeddedFileProviderExtensions)).GetReferencedAssemblies()) {
			//	try {
			//		var assembly = Assembly.Load(assemblyName);
			//		var providerContainer = assembly.GetTypes().FirstOrDefault(t => typeof(IEmbeddedFileProviderContainer).IsAssignableFrom(t) && !t.GetTypeInfo().IsInterface);
			//		if(providerContainer != null) {
			//			var container = Activator.CreateInstance(providerContainer);
			//			yield return (container as IEmbeddedFileProviderContainer).FileProvider;
			//		}
			//	} finally {
			//	}
#endif
		}
	}
}