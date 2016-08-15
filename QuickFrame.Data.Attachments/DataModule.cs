﻿using Autofac;
using QuickFrame.Data.Services;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;

namespace QuickFrame.Data.Attachments {

	public class DataModule : Autofac.Module {

		protected override void Load(ContainerBuilder builder) {
			builder.RegisterType(typeof(AttachmentsContext)).InstancePerLifetimeScope();
			foreach(var obj in typeof(AttachmentsContext).Assembly.GetTypes().Where(t => t.GetTypeInfo().GetCustomAttribute<ExportAttribute>() != null || typeof(DataService<,,>).IsAssignableFrom(t))) {
				ExportAttribute att = obj.GetCustomAttribute<ExportAttribute>();
				if(att?.ContractType != null) {
					builder.RegisterType(obj).As(att.ContractType);
				} else {
					foreach(var intf in obj.GetInterfaces())
						builder.RegisterType(obj).As(intf);
				}
			}
		}
	}
}