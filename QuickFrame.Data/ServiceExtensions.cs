using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuickFrame.Data.Interfaces.Dtos;
using System;
using System.Linq;
using System.Reflection;

namespace QuickFrame.Data {

	///<summary>A static class that provides methods for registering services and other setup options for this assembly.</summary>
	public static class ServiceExtensions {

		///<summary>Static extension method used to register services and set up options for this assembly.</summary>
		///<param name="services">
		///	The <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection">IServiceCollection</see> on which this method operates
		///</param>
		///<param name="configuration">
		///	An <see cref="Microsoft.Extensions.Configuration.IConfigurationRoot">IConfigurationRoot</see> representing the values within the appsettings.json.
		///</param>
		///<returns>
		///	The <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection">IServiceCollection</see> on which this method operates, allowing it to be chained with other registration methods.
		///</returns>
		public static IServiceCollection AddQuickFrameData(this IServiceCollection services, IConfigurationRoot configuration) {
			services.Configure<DataOptions>(dataConfig => {
				dataConfig.Load(configuration);
			});

			return services;
		}

		public static IServiceCollection AddExpressMapperObjects(this IServiceCollection services, Assembly assembly) {
			foreach(var objType in assembly.GetTypes().Where(t => typeof(IDataTransferObjectCore).IsAssignableFrom(t) && !t.IsInterface && !t.IsGenericType)) {
				var obj = Activator.CreateInstance(objType);
				(obj as IDataTransferObjectCore).Register();
			}

			return services;
		}
	}
}