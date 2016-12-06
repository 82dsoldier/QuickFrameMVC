using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using QuickFrame.IO;

namespace QuickFrame.Tests
{
    public class IOTests
    {
		[Theory]
		[InlineData(@"C:\Temp\QuickFrameMVC")]
		//[InlineData(@"\\deac.pad.local\home\Drew.Burchett")]
		public void DirectoryEnumeratorTest(string directory) {
			var writer = System.IO.File.CreateText(@"C:\temp\dir.txt");
			var ct = 0;
			foreach(var obj in Directory.EnumerateDirectory(directory, System.IO.SearchOption.AllDirectories)) {
				writer.WriteLine(obj);
				ct++;
				if(ct == 100) {
					writer.Flush();
					ct = 0;
				}
			}
			writer.Close();
		}
    }
}
