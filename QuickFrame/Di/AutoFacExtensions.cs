using Autofac;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using System.Linq;

namespace QuickFrame.Di {

	/// <summary>
	/// Provides the method for automatic registration of exported classes
	/// </summary>
	public static class AutofacExtensions {

		public static IServiceCollection AddAutofac(this IServiceCollection services) {
			//var partList = DefaultAssemblyPartDiscoveryProvider.DiscoverAssemblyParts(assemblyName);

			//foreach(var part in partList) {
			//	//foreach(var obj in part.GetType().GetTypes()) {
			//	//}
			//}
			var dependencyContext = DependencyContext.Default;

			foreach(var compilationLibrary in dependencyContext.RuntimeLibraries.Where(lib => !lib.Name.StartsWith("Microsoft")
			&& !lib.Name.StartsWith("NuGet")
			&& !lib.Name.StartsWith("System")
			&& !lib.Name.StartsWith("runtime"))) {
				var partList = DefaultAssemblyPartDiscoveryProvider.DiscoverAssemblyParts(compilationLibrary.Name);
				foreach(var part in partList) {
				}
				foreach(var assembly in compilationLibrary.Assemblies) {
					//var loadedAssembly = Assembly.LoadFile(assembly);
					//var moduleList = loadedAssembly.GetTypes().Where(t => typeof(Autofac.Module).IsAssignableFrom(t));

					//if(moduleList == null)
					//	ComponentContainer.RegisterAssembly(loadedAssembly);
					//else
					//	ComponentContainer.Builder.RegisterAssemblyModules(loadedAssembly);
				}
			}

			return services;
		}
	}
}