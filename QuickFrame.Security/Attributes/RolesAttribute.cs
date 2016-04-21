using System;

namespace QuickFrame.Security.Attributes {

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class RolesAttribute : Attribute {
		private readonly string[] _allowedRoles;
		private readonly string[] _deniedRoles;

		public string[] AllowedRoles => _allowedRoles;
		public string[] DeniedRoles => _deniedRoles;

		public RolesAttribute(string[] allowedRoles = null, string[] deniedRoles = null) {
			_allowedRoles = allowedRoles;
			_deniedRoles = deniedRoles;
		}
	}
}