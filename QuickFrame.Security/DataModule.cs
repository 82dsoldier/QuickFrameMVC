using Autofac;
using Microsoft.AspNetCore.Identity;
using QuickFrame.Data.Services;
using QuickFrame.Security.AccountControl;
using QuickFrame.Security.AccountControl.Data;
using QuickFrame.Security.AccountControl.Data.Models;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;

namespace QuickFrame.Security {

	public class DataModule : Autofac.Module {

		protected override void Load(ContainerBuilder builder) {
			builder.RegisterType(typeof(SecurityContext)).InstancePerLifetimeScope();
			foreach(var obj in typeof(SecurityContext).Assembly.GetTypes().Where(t => t.GetTypeInfo().GetCustomAttribute<ExportAttribute>() != null || typeof(DataService<,,>).IsAssignableFrom(t))) {
				ExportAttribute att = obj.GetCustomAttribute<ExportAttribute>();
				if(att?.ContractType != null) {
					builder.RegisterType(obj).As(att.ContractType);
				} else {
					foreach(var intf in obj.GetInterfaces())
						builder.RegisterType(obj).As(intf);
				}
			}
			builder.RegisterType(typeof(QuickFrameIdentityErrorDescriber));
			builder.RegisterType(typeof(GroupManager<SiteGroup>));
			builder.RegisterType(typeof(QuickFrameRoleStore)).As<IRoleStore<SiteRole>>();
		}
	}
}