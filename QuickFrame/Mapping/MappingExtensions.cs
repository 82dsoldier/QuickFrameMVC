using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

#if NETCOREAPP1_0
using System.Runtime.Loader;
#endif

namespace QuickFrame.Mapping {

	public static class MappingExtensions {

		public static IServiceCollection AddMapping(this IServiceCollection services) {
			ApplicationEnvironment env = PlatformServices.Default.Application;
			foreach(var file in Directory.GetFiles(env.ApplicationBasePath).Where(f => f.EndsWith(".dll"))) {
				//var assemblyName = new AssemblyName(file);
				Assembly assembly = null;
				try {
					Assembly.LoadFile(file);
				} catch { }
				if(assembly != null) {
					IEnumerable<Type> typeList = null;
					try {
						typeList = assembly.GetTypes().Where(t => t.GetTypeInfo().GetCustomAttribute<ExpressMapAttribute>() != null);
					} catch {
					}
					if(typeList != null) {
						foreach(var obj in typeList)
							MapRegistration.Register(obj);
					}
				}
			}

			//#if NETCOREAPP1_0
			//			var libraryManager = services.FirstOrDefault(s => s.ServiceType == typeof(ILibraryManager)).ImplementationInstance as ILibraryManager;

			//			foreach (var library in libraryManager.GetLibraries()) {
			//				Assembly assembly = null;
			//				try {
			//					assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(library.Path);
			//				} catch {
			//					continue;
			//				}

			//				if (assembly != null)
			//					foreach (var obj in assembly.GetTypes().Where(t => t.GetTypeInfo().GetCustomAttribute<ExpressMapAttribute>() != null))
			//						MapRegistration.Register(obj);
			//			}

			//#else
			//			foreach(var assemblyName in Assembly.GetAssembly(typeof(MappingExtensions)).GetReferencedAssemblies()) {
			//				Assembly assembly = null;
			//				try {
			//					assembly = Assembly.Load(assemblyName.Name);
			//				} catch {
			//				}
			//				if(assembly != null) {
			//					IEnumerable<Type> typeList = null;
			//					try {
			//						typeList = assembly.GetTypes().Where(t => t.GetTypeInfo().GetCustomAttribute<ExpressMapAttribute>() != null);
			//					} catch {
			//					}
			//					if(typeList != null) {
			//						foreach(var obj in typeList)
			//							MapRegistration.Register(obj);
			//					}
			//				}
			//			}
			//#endif
			return services;
		}
	}
}