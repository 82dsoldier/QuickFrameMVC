using System;
using System.Reflection;

namespace QuickFrame.Mapping {

	public static class MapRegistration {

		public static void Register(Type objType) {
			var obj = Activator.CreateInstance(objType);
			var registerMethod = objType.GetTypeInfo().GetMethod("Register");
			registerMethod.Invoke(obj, null);
		}

		//public static void Compile() => Mapper.Compile();
	}
}