using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuickFrame.Configuration;
using System;

namespace QuickFrame {

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
		public static IServiceCollection AddQuickFrame(this IServiceCollection services, IConfigurationRoot configuration) {
			services
			.Configure<SmtpOptions>(options => {
				options.FromAddress = configuration["SmtpOptions:fromAddress"];
				int port = 25;
				Int32.TryParse(configuration["SmtpOptions:port"], out port);
				options.Port = port;
				options.Server = configuration["SmtpOptions:server"];
			});
			return services;
		}
	}
}