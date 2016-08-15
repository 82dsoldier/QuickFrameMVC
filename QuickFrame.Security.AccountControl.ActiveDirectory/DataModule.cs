using Autofac;
using ExpressMapper;
using Microsoft.AspNetCore.Identity;
using QuickFrame.Data.Services;
using QuickFrame.Security.AccountControl.ActiveDirectory.AdLookup;
using QuickFrame.Security.AccountControl.Data.Models;
using QuickFrame.Security.AccountControl.Interfaces;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;

namespace QuickFrame.Security.AccountControl.ActiveDirectory {

	public class DataModule : Autofac.Module {

		protected override void Load(ContainerBuilder builder) {
			foreach(var obj in typeof(AdUserStore).Assembly.GetTypes().Where(t => t.GetTypeInfo().GetCustomAttribute<ExportAttribute>() != null || typeof(DataService<,,>).IsAssignableFrom(t))) {
				ExportAttribute att = obj.GetCustomAttribute<ExportAttribute>();
				if(att?.ContractType != null) {
					builder.RegisterType(obj).As(att.ContractType);
				} else {
					foreach(var intf in obj.GetInterfaces())
						builder.RegisterType(obj).As(intf);
				}
			}

			builder.RegisterType(typeof(AdUserStore)).As<IUserStore<SiteUser>>();
			builder.RegisterType(typeof(AdGroupStore)).As<IGroupStore<SiteGroup>>();
			Mapper.RegisterCustom<Person, SiteUser, PersonSiteUserMap>();
		}
	}
}