using QuickFrame.Security.AccountControl.ActiveDirectory.AdLookup.Interfaces;
using System.Collections.Generic;

namespace QuickFrame.Security.AccountControl.ActiveDirectory.AdLookup {

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
}