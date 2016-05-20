using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using System.ComponentModel.Composition;
using System.Reflection;
using QuickFrame.Data;
using QuickFrame.Attachments.Data;

namespace PmData
{
    public class DataModule : Autofac.Module
    {
		protected override void Load(ContainerBuilder builder) {
			builder.RegisterType(typeof(AttachmentsContext)).InstancePerLifetimeScope();
			foreach (var obj in typeof(AttachmentsContext).Assembly.GetTypes().Where(t => t.GetCustomAttribute<ExportAttribute>() != null || typeof(DataServiceCore<,,>).IsAssignableFrom(t))) {
				ExportAttribute att = obj.GetCustomAttribute<ExportAttribute>();
				if (att?.ContractType != null) {
					builder.RegisterType(obj).As(att.ContractType);
				} else {
					foreach (var intf in obj.GetInterfaces())
						builder.RegisterType(obj).As(intf);
				}
			}
		}
	}
}
