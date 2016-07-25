using Autofac;
using System;
using System.Composition;
using System.Reflection;

namespace QuickFrame.Di {

	public static class ComponentContainer {
		private static IContainer _container;
		private static ContainerBuilder _containerBuilder;

		public static ContainerBuilder Builder
		{
			get
			{
				if(_containerBuilder == null)
					_containerBuilder = new ContainerBuilder();
				return _containerBuilder;
			}
		}

		public static IServiceProvider ServiceProvider
		{
			get
			{
				if(_container == null)
					_container = _containerBuilder.Build();
				return _container.Resolve<IServiceProvider>();
			}
		}

		public static ComponentFactory<TObject> Component<TObject>() => new ComponentFactory<TObject>(_container);
		public static ComponentFactoryEx Component(Type typeToResolve) => new ComponentFactoryEx(_container, typeToResolve);

		public static void RegisterAssembly(Assembly assembly) {
			if(assembly == null)
				return;
			try {
				foreach(var obj in assembly.GetTypes()) {
					ExportAttribute att = null;
					try {
						att = obj.GetTypeInfo().GetCustomAttribute<ExportAttribute>();
					} catch {
					}

					if(att != null) {
						if(att.ContractType != null) {
							if(att.ContractType == obj)
								Builder.RegisterType(obj).InstancePerLifetimeScope();
							else
								Builder.RegisterType(obj).As(att.ContractType);
						} else {
							foreach(var intf in obj.GetInterfaces())
								Builder.RegisterType(obj).As(intf);
						}
					}
				}
			} catch {
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

	public class ComponentFactoryEx : IDisposable {
		protected object CurrentObject;
		protected ILifetimeScope CurrentScope;

		public object Current => CurrentObject;

		public ComponentFactoryEx(IContainer container, Type typeToResolve) {
			CurrentScope = container.BeginLifetimeScope();
			CurrentObject = CurrentScope.Resolve(typeToResolve);
		}

		public void Dispose() {
			(CurrentObject as IDisposable)?.Dispose();
			CurrentScope.Dispose();
		}
	}
}