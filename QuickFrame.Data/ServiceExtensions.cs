using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuickFrame.Di;

namespace QuickFrame.Data {

	public static class ServiceExtensions {
		private static ContainerBuilder _builder => ComponentContainer.Builder;

		public static IServiceCollection AddQuickFrameData(this IServiceCollection services, IConfigurationRoot configuration) {
			services.Configure<DataOptions>(dataConfig => {
				dataConfig.Load(configuration);
			});
			return services;
		}
	}
}