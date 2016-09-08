using Autofac;
using System;
using System.Composition;
using System.Reflection;

/// <include file='Doc/Documentation.xml' path='QuickFrame/documentation[@name="DI"]'/> 
namespace QuickFrame.Di {

	/// <include file='Doc/Documentation.xml' path='QuickFrame/Di/documentation[@name="ComponentContainer"]'/> 
	public static class ComponentContainer {
		private static IContainer _container;
		private static ContainerBuilder _containerBuilder;

		/// <include file='Doc/Documentation.xml' path='QuickFrame/Di/ComponentContainer/documentation[@name="Builder"]'/> 
		public static ContainerBuilder Builder
		{
			get
			{
				if(_containerBuilder == null)
					_containerBuilder = new ContainerBuilder();
				return _containerBuilder;
			}
		}

		/// <include file='Doc/Documentation.xml' path='QuickFrame/Di/ComponentContainer/documentation[@name="ServiceProvider"]'/> 
		public static IServiceProvider ServiceProvider
		{
			get
			{
				if(_container == null)
					_container = _containerBuilder.Build();
				return _container.Resolve<IServiceProvider>();
			}
		}

		/// <include file='Doc/Documentation.xml' path='QuickFrame/Di/ComponentContainer/documentation[@name="Component{TObject}"]'/> 
		public static ComponentFactory<TObject> Component<TObject>() => new ComponentFactory<TObject>(_container);

		/// <include file='Doc/Documentation.xml' path='QuickFrame/Di/ComponentContainer/documentation[@name="Component"]'/> 
		public static ComponentFactoryEx Component(Type typeToResolve) => new ComponentFactoryEx(_container, typeToResolve);

		/// <include file='Doc/Documentation.xml' path='QuickFrame/Di/ComponentContainer/documentation[@name="Register"]'/> 
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

	/// <include file='Doc/Documentation.xml' path='QuickFrame/Di/documentation[@name="ComponentFactory"]'/> 
	public class ComponentFactory<TObject> : IDisposable {
		private TObject CurrentObject;
		private ILifetimeScope CurrentScope;

		/// <include file='Doc/Documentation.xml' path='QuickFrame/Di/ComponentFactory/documentation[@name="Current"]'/> 
		public TObject Current => (TObject)CurrentObject;

		/// <include file='Doc/Documentation.xml' path='QuickFrame/Di/ComponentFactory/documentation[@name="ComponentFactory"]'/> 
		public ComponentFactory(IContainer container) {
			CurrentScope = container.BeginLifetimeScope();

			CurrentObject = CurrentScope.Resolve<TObject>();
		}

		/// <include file='Doc/Documentation.xml' path='QuickFrame/Di/ComponentFactory/documentation[@name="Component"]'/> 
		public TObject Component => CurrentObject;

		/// <include file='Doc/Documentation.xml' path='QuickFrame/Di/ComponentFactory/documentation[@name="Dispose"]'/> 
		public void Dispose() {
			(CurrentObject as IDisposable)?.Dispose();
			CurrentScope.Dispose();
		}
	}

	/// <include file='Doc/Documentation.xml' path='QuickFrame/Di/documentation[@name="ComponentFactoryEx"]'/> 
	public class ComponentFactoryEx : IDisposable {
		private object CurrentObject;
		private ILifetimeScope CurrentScope;

		/// <include file='Doc/Documentation.xml' path='QuickFrame/Di/ComponentFactoryEx/documentation[@name="Current"]'/> 
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