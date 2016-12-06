using QuickFrame.Security.AccountControl.ActiveDirectory.AdLookup.Interfaces;
using System;

namespace QuickFrame.Security.AccountControl.ActiveDirectory.AdLookup {

	public class GuidTransformer : ITransformData<byte[], string> {

		public string Call(byte[] arg) {
			return arg != null ? new Guid(arg).ToString() : null;
		}
	}
}