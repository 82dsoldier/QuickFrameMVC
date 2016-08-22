using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace QuickFrame.Security.AccountControl.ActiveDirectory.AdLookup.Interfaces {

	public interface ITransformData<TArg, TResult> {

		TResult Call(TArg arg);
	}

	public class SidTransformer : ITransformData<byte[], string> {

		public string Call(byte[] arg) {
			SecurityIdentifier sid = new SecurityIdentifier(arg, 0);
			return sid.Value;
		}
	}

	public class GroupTypeTransformer : ITransformData<int[], string[]> {

		private static Dictionary<long, string> GroupTypes = new Dictionary<long, string> {
			{0x02, "Global" },
			{0x04, "Domain Local" },
			{0x08, "Universal" },
			{0x80000000, "Secrity Enabled" }
		};

		public string[] Call(int[] arg) {
			var typeList = new List<string>();
			foreach(var val in arg) {
				typeList.Add(GroupTypes[val & 0x000000FF]);
				if((val & 0xFF00000000) != 0)
					typeList.Add(GroupTypes[0x80000000]);
			}
			return typeList.ToArray();
		}
	}

	public class GuidTransformer : ITransformData<byte[], string> {

		public string Call(byte[] arg) {
			return arg != null ? new Guid(arg).ToString() : null;
		}
	}
}