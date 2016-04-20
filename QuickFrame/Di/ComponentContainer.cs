using Autofac;
using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;

namespace QuickFrame.Di {

	public static class ComponentContainer {
		private static IContainer _container;
		private static ContainerBuilder _containerBuilder;

		public static ContainerBuilder Builder {
			get {
				if (_containerBuilder == null)
					_containerBuilder = new ContainerBuilder();
				return _containerBuilder;
			}
		}

		public static IServiceProvider ServiceProvider {
			get {
				if (_container == null)
					_container = _containerBuilder.Build();
				return _container.Resolve<IServiceProvider>();
			}
		}

		public static ComponentFactory<TObject> Component<TObject>() => new ComponentFactory<TObject>(_container);

		public static void RegisterAssembly(Assembly assembly) {
			if (assembly == null) return;

			foreach (var obj in assembly.GetTypes().Where(t => t.GetCustomAttribute<ExportAttribute>() != null)) {
				ExportAttribute att = obj.GetCustomAttribute<ExportAttribute>();
				if (att.ContractType != null) {
					if (att.ContractType == obj)
						Builder.RegisterType(obj).InstancePerLifetimeScope();
					else
						Builder.RegisterType(obj).As(att.ContractType);
				} else {
					foreach (var intf in obj.GetInterfaces())
						Builder.RegisterType(obj).As(intf);
				}
			}
		}
	}

	public class ComponentFactory<TObject> : IDisposable {
		protected TObject CurrentObject;
		protected ILifetimeScope CurrentScope;
		public TObject Current => (TObject)CurrentObject;

		public ComponentFactory(IContainer container) {
			CurrentScope = container.BeginLifetimeScope();

			CurrentObject = CurrentScope.Resolve<TObject>();
		}

		public TObject Component => CurrentObject;

		public void Dispose() {
			(CurrentObject as IDisposable)?.Dispose();
			CurrentScope.Dispose();
		}
	}
}