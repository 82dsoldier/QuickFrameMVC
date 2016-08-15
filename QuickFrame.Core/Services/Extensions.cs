using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Options;
using QuickFrame.Data;
using QuickFrame.Di;
using QuickFrame.Interfaces;
using QuickFrame.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QuickFrame.Core.Services {

	public static class Extensions {

		public static IServiceCollection AddApplicationAssemblies(this IServiceCollection serviceCollection) {
			foreach(var assembly in GetAssemblyList()) {
				try {
					var module = assembly.GetTypes().FirstOrDefault(t => typeof(Autofac.Module).IsAssignableFrom(t));

					if(module != null)
						ComponentContainer.Builder.RegisterAssemblyModules(assembly);
					else
						ComponentContainer.RegisterAssembly(assembly);

					foreach(var mod in assembly.GetTypes().Where(t => t.GetCustomAttribute<ExpressMapAttribute>() != null)) {
						var obj = Activator.CreateInstance(mod);
						var registerMethod = mod.GetTypeInfo().GetMethod("Register");
						registerMethod.Invoke(obj, null);
					}

					foreach(var mod in assembly.GetTypes().Where(t => typeof(IEmbeddedFileProviderContainer).IsAssignableFrom(t) && !t.IsInterface)) {
						serviceCollection.Configure<RazorViewEngineOptions>(options => {
							var obj = Activator.CreateInstance(mod);
							options.FileProviders.Add((obj as IEmbeddedFileProviderContainer).FileProvider);
						});
					}
				} catch {
				}
			}
			return serviceCollection;
		}

		public static IApplicationBuilder UseEmbeddedFileProviders(this IApplicationBuilder app) {
			IOptions<RazorViewEngineOptions> razorViewEngineOptions =
				app.ApplicationServices.GetService<IOptions<RazorViewEngineOptions>>();

			foreach(var assembly in GetAssemblyList()) {
				if(assembly != null) {
					foreach(var mod in assembly.GetTypes().Where(t => typeof(IEmbeddedFileProviderContainer).IsAssignableFrom(t) && !t.IsInterface)) {
						var obj = Activator.CreateInstance(mod);
						razorViewEngineOptions.Value.FileProviders.Add((obj as IEmbeddedFileProviderContainer).FileProvider);
					}
				}
			}
			return app;
		}

		private static IEnumerable<Assembly> GetAssemblyList() {
			var deps = DependencyContext.Default;
			foreach(var compilationLibrary in deps.CompileLibraries.Where(obj => !obj.Name.StartsWith("Microsoft")
			&& !obj.Name.StartsWith("System")
			&& !obj.Name.StartsWith("NuGet")
			&& !obj.Name.StartsWith("runtime")
			&& !obj.Name.StartsWith("Autofac")
			&& !obj.Name.StartsWith("EntityFramework")
			&& !obj.Name.StartsWith("Newtonsoft")
			&& !obj.Name.StartsWith("QuickFrame"))) {
				Assembly assembly = null;
				try {
					assembly = Assembly.Load(new AssemblyName(compilationLibrary.Name));
				} catch {
				}
				yield return assembly;
			}
		}
	}
}