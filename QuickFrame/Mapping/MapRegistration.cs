using ExpressMapper;
using System;

namespace QuickFrame.Mapping {

	public static class MapRegistration {

		public static void Register(Type objType) {
			var obj = Activator.CreateInstance(objType);
			var registerMethod = objType.GetMethod("Register");
			registerMethod.Invoke(obj, null);
		}

		public static void Compile() => Mapper.Compile();
	}
}