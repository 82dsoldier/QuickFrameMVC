using Autofac;
using Microsoft.Extensions.DependencyInjection;
using QuickFrame.Data.Services;
using QuickFrame.Di;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;

namespace QuickFrame.Data.Attachments {

	public static class ServiceExtensions {
		private static ContainerBuilder _builder => ComponentContainer.Builder;

		public static IServiceCollection AddQuickFrameAttachments(this IServiceCollection collection) {
			_builder.RegisterType(typeof(AttachmentsContext)).InstancePerLifetimeScope();
			foreach(var obj in typeof(AttachmentsContext).Assembly.GetTypes().Where(t => t.GetTypeInfo().GetCustomAttribute<ExportAttribute>() != null || typeof(DataService<,,>).IsAssignableFrom(t))) {
				ExportAttribute att = obj.GetCustomAttribute<ExportAttribute>();
				if(att?.ContractType != null) {
					_builder.RegisterType(obj).As(att.ContractType);
				} else {
					foreach(var intf in obj.GetInterfaces())
						_builder.RegisterType(obj).As(intf);
				}
			}
			return collection;
		}
	}
}