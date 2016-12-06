using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static QuickFrame.Api.Kernel32;
using static QuickFrame.Api.mpr;

namespace QuickFrame.IO
{
    public static class Path
    {
		public static bool IsDirectory(string pathName) {
			var attr = GetFileAttributes(pathName);
			if(attr == INVALID_RETURN_VALUE)
				return false;
			return (attr & FILE_ATTRIBUTE_DIRECTORY) == FILE_ATTRIBUTE_DIRECTORY;
		}

		public static bool IsDotDir(string pathName) {
			var ch = pathName[pathName.Length - 1];
			return ch == '.';
		}

		public static string GetFileName(string filePath) {
			for(int i = filePath.Length - 1; i >= 0; i--) {
				char ch = filePath[i];
				if(ch == '\\' || ch == '/') {
					int count = (filePath.Length - i) - 1;
					return filePath.Substring(i + 1, count);
				}
			}

			return filePath;
		}

		public static string GetUncPath(string path) {
			if(String.IsNullOrWhiteSpace(path)) 
				throw new ArgumentException("Path must not be an empty string");

			if(!System.IO.Path.IsPathRooted(path))
				throw new ArgumentException("Path is not rooted");

			if(path.StartsWith(@"\\"))
				return path;

			var driveLetter = System.IO.Directory.GetDirectoryRoot(path).Replace(System.IO.Path.DirectorySeparatorChar.ToString(), "");

			var uncName = GetUncName(driveLetter);

			return path.Replace(driveLetter, uncName);
		}
	}
}
