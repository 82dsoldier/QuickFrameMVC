using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuickFrame.Api;
using static QuickFrame.Api.Kernel32;
using System.ComponentModel;

namespace QuickFrame.IO
{
    public class File
    {
		public static long Length(string fileName) {
			var wfd = new WIN32_FIND_DATA();
			var handle = FindFirstFile(fileName, out wfd);

			if(handle == INVALID_HANDLE_VALUE)
				throw new Win32Exception();

			long ret = ((long)wfd.nFileSizeHigh) << 32 | ((long)wfd.nFileSizeLow);

			FindClose(handle);

			return ret;
		}
	}
}
