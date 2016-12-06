using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace QuickFrame.IO.Tests
{
    public class IOTests
    {
		[Theory]
		[InlineData(@"C:\Source Code\QuickFrameMVC\QuickFrame.Tests\global.json")]
		[InlineData(@"S:\Controlled Documents\ENGINEERING DOCUMENTS\Equivalency Subsitution Evaluations\ESE-FA1200-0003 R0 CAAS 20 KVA UPS Part Number Change.pdf")]
		[InlineData(@"\\deac.pad.local\Shared\Controlled Documents\ENGINEERING DOCUMENTS\Equivalency Subsitution Evaluations\ESE-FA1200-0003 R0 CAAS 20 KVA UPS Part Number Change.pdf")]
		[InlineData(@"ThisIsAtest.pdf")]
		public void GetFileName(string dir) {
			string file = QuickFrame.IO.Path.GetFileName(dir);
			Assert.True(!String.IsNullOrEmpty(file));
			Assert.True(dir.Contains(file));
		}

		[Theory]
		[InlineData(@"C:\Source Code\QuickFrameMVC\QuickFrame.Tests\global.json")]
		[InlineData(@"S:\Controlled Documents\ENGINEERING DOCUMENTS\Equivalency Subsitution Evaluations\ESE-FA1200-0003 R0 CAAS 20 KVA UPS Part Number Change.pdf")]
		[InlineData(@"\\deac.pad.local\Shared\Controlled Documents\ENGINEERING DOCUMENTS\Equivalency Subsitution Evaluations\ESE-FA1200-0003 R0 CAAS 20 KVA UPS Part Number Change.pdf")]
		public void GetFileSize(string file) {
			long length = File.Length(file);

			Assert.True(length > 0);
		}
	}
}
