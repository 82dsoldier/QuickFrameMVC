using QuickFrame.Security.AccountControl.ActiveDirectory.AdLookup.Interfaces;
using System.Security.Principal;

namespace QuickFrame.Security.AccountControl.ActiveDirectory.AdLookup {

	public class SidTransformer : ITransformData<byte[], string> {

		public string Call(byte[] arg) {
			SecurityIdentifier sid = new SecurityIdentifier(arg, 0);
			return sid.Value;
		}
	}
}