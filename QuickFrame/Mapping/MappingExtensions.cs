using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Web.CodeGeneration.DotNet;
using System.Linq;
using System.Reflection;

#if NETCOREAPP1_0
using System.Runtime.Loader;
#endif

namespace QuickFrame.Mapping {

	public static class MappingExtensions {

		public static IServiceCollection AddMapping(this IServiceCollection services) {
#if NETCOREAPP1_0
			var libraryManager = services.FirstOrDefault(s => s.ServiceType == typeof(ILibraryManager)).ImplementationInstance as ILibraryManager;

			foreach (var library in libraryManager.GetLibraries()) {
				Assembly assembly = null;
				try {
					assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(library.Path);
				} catch {
					continue;
				}

				if (assembly != null)
					foreach (var obj in assembly.GetTypes().Where(t => t.GetTypeInfo().GetCustomAttribute<ExpressMapAttribute>() != null))
						MapRegistration.Register(obj);
			}

#else
			foreach(var assemblyName in Assembly.GetAssembly(typeof(MappingExtensions)).GetReferencedAssemblies()) {
				var assembly = Assembly.Load(assemblyName);
				foreach(var obj in assembly.GetTypes().Where(t => t.GetTypeInfo().GetCustomAttribute<ExpressMapAttribute>() != null))
					MapRegistration.Register(obj);
			}
#endif
			return services;
		}
	}
}