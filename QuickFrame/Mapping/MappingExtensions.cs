using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using System.Linq;
using System.Reflection;

namespace QuickFrame.Mapping {

	public static class MappingExtensions {

		public static IServiceCollection AddMapping(this IServiceCollection services) {
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

				if (assembly != null)
					foreach (var obj in assembly.GetTypes().Where(t => t.GetCustomAttribute<ExpressMapAttribute>() != null))
						MapRegistration.Register(obj);
			}

			return services;
		}
	}
}