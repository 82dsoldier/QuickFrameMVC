using QuickFrame.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Console.Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
			//var writer = System.IO.File.CreateText(@"C:\temp\dir.txt");
			var ct = 0;
			foreach(var obj in Directory.EnumerateDirectory(@"\\deac.pad.local\shared\Controlled Documents", System.IO.SearchOption.AllDirectories)) {
				System.Console.WriteLine(obj);
				ct++;
				//writer.WriteLine(obj);
				//ct++;
				//if(ct == 100) {
				//	writer.Flush();
				//	ct = 0;
				//}
			}
			System.Console.WriteLine(ct);
			System.Console.ReadKey();
			//writer.Close();
		}
	}
}
