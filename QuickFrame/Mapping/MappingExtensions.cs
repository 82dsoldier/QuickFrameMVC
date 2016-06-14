using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using QuickFrame.Utility;
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

			foreach(var assembly in IO.GetAssemblies()) {

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
			return services;
		}
	}
}