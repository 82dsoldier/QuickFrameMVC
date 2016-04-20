using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using System.Linq;
using System.Reflection;

namespace QuickFrame.Di {

	/// <summary>
	/// Provides the method for automatic registration of exported classes
	/// </summary>
	public static class AutofacExtensions {

		public static IServiceCollection AddAutofac(this IServiceCollection services) {
			var assemblyLoadContextAccessor = services.FirstOrDefault(s => s.ServiceType == typeof(IAssemblyLoadContextAccessor)).ImplementationInstance as IAssemblyLoadContextAccessor;
			var libraryManager = services.FirstOrDefault(s => s.ServiceType == typeof(ILibraryManager)).ImplementationInstance as ILibraryManager;
			var loadContext = assemblyLoadContextAccessor.Default;

			foreach (var library in libraryManager.GetLibraries()) {
				Assembly assembly = null;
				try {
					assembly = loadContext.Load(library.Name);
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

			return services;
		}
	}
}