using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QuickFrame.Api
{
    public static class mpr
    {
		[DllImport("mpr.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern int WNetGetConnection(
			   [MarshalAs(UnmanagedType.LPTStr)] string localName,
			   [MarshalAs(UnmanagedType.LPTStr)] StringBuilder remoteName,
			   ref int length);

		public static string GetUncName(string driveLetter) {
			if(!driveLetter.EndsWith(":"))
				driveLetter = $"{driveLetter}:";

			var sb = new StringBuilder(512);
			var size = 512;

			WNetGetConnection(driveLetter, sb, ref size);

			return sb.ToString();
		}
	}
}
