using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace QuickFrame.IO.Api
{
    public static class Kernel32
    {
		public static int FILE_ATTRIBUTE_ARCHIVE = 0X20;
		public static int FILE_ATTRIBUTE_COMPRESSED = 0x800;
		public static int FILE_ATTRIBUTE_DEVICE = 0x40;
		public static int FILE_ATTRIBUTE_DIRECTORY = 0x10;
		public static int FILE_ATTRIBUTE_HIDDEN = 0x2;
		public static int FILE_ATTRIBUTE_INTEGRITY_STREAM = 0x8000;
		public static int FILE_ATTRIBUTE_NORMAL = 0x80;
		public static int FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 0x2000;
		public static int FILE_ATTRIBUTE_NO_SCRUB_DATA = 0x20000;
		public static int FILE_ATTRIBUTE_OFFLINE = 0x1000;
		public static int FILE_ATTRIBUTE_READONLY = 0x1;
		public static int FILE_ATTRIBUTE_REPARSE_POINT = 0x400;
		public static int FILE_ATTRIBUTE_SPARSE_FILE = 0x200;
		public static int FILE_ATTRIBUE_SYSTEM = 0X4;
		public static int FILE_ATTRIBUTE_TEMPORARY = 0x100;
		public static int FILE_ATTRIBUTE_VIRTUAL = 0x10000;

		public static IntPtr INVALID_HANDLE_VALUE = new IntPtr(0xFFFFFF);
		public static int INVALID_RETURN_VALUE = 0xFFFFFF;

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		public static extern IntPtr FindFirstFile(string lpFileName, out WIN32_FIND_DATA lpFindFileData);
		
		[DllImport("kernel32.dll", CharSet=CharSet.Unicode)]
		public static extern bool FindNextFile(IntPtr hFindFile, out WIN32_FIND_DATA lpFindFileData);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool FindClose(IntPtr hFindFile);

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern uint GetFileAttributes(string lpFileName);

	}
}
