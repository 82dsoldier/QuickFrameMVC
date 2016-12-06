using QuickFrame.Api;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static QuickFrame.Api.Kernel32;

namespace QuickFrame.IO
{
    public static class Directory
    {
		public static IEnumerable<string> EnumerateDirectory(string directoryName, System.IO.SearchOption searchOption = System.IO.SearchOption.TopDirectoryOnly) {

			var wfd = new WIN32_FIND_DATA();
			var dirQueue = new Queue<string>();
			dirQueue.Enqueue(directoryName);

			while(dirQueue.Count > 0) {
				var currentDir = dirQueue.Dequeue();
				var searchPath = Combine(currentDir, "*");

				IntPtr findHandle = FindFirstFile(searchPath, out wfd);

				if(findHandle == INVALID_HANDLE_VALUE)
					throw new Win32Exception();

				do {
					var ch = wfd.cFileName[wfd.cFileName.Length - 1];
					if(ch != '.') {
						var currentFile = Combine(currentDir, wfd.cFileName);
						yield return currentFile;
						if(searchOption == System.IO.SearchOption.AllDirectories && (wfd.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) == FILE_ATTRIBUTE_DIRECTORY)
							dirQueue.Enqueue(currentFile);
					}
				} while(FindNextFile(findHandle, out wfd));

				FindClose(findHandle);
			}
		}

		private static string Combine(string first, string second) {
			var ch = first[first.Length - 1];
			if(ch != '\\')
				return $@"{first}\{second}";
			return $"{first}{second}";
		}

    }
}
