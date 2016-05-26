using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Web.CodeGeneration.DotNet;
using System.Linq;
using System.Reflection;

#if NETCOREAPP1_0
using System.Runtime.Loader;
#endif

namespace QuickFrame.Di {

	/// <summary>
	/// Provides the method for automatic registration of exported classes
	/// </summary>
	public static class AutofacExtensions {

		public static IServiceCollection AddAutofac(this IServiceCollection services) {
#if NETCOREAPP1_0

			var libraryManager = services.FirstOrDefault(s => s.ServiceType == typeof(ILibraryManager)).ImplementationInstance as ILibraryManager;

			foreach (var library in libraryManager.GetLibraries()) {
				Assembly assembly = null;
				try {
					assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(library.Path);
				} catch {
					continue;
				}

				if (assembly != null) {
					var module = assembly.GetTypes().FirstOrDefault(t => typeof(Autofac.Module).IsAssignableFrom(t));

					if (module != null)
						ComponentContainer.Builder.RegisterAssemblyModules(assembly);
					else
						ComponentContainer.RegisterAssembly(assembly);
				}
			}

			ComponentContainer.Builder.Populate(services);
#else
			foreach(var assemblyName in Assembly.GetAssembly(typeof(AutofacExtensions)).GetReferencedAssemblies()) {
				var assembly = Assembly.Load(assemblyName);
				if(assembly != null) {
					var module = assembly.GetTypes().FirstOrDefault(t => typeof(Autofac.Module).IsAssignableFrom(t));

					if(module != null)
						ComponentContainer.Builder.RegisterAssemblyModules(assembly);
					else
						ComponentContainer.RegisterAssembly(assembly);
				}
			}
#endif
			return services;
		}
	}
}