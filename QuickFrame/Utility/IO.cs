using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace QuickFrame.Utility
{
    public static class IO
    {
		public static IEnumerable<Assembly> GetAssemblies() {
			var deps = DependencyContext.Default;
			foreach(var compilationLibrary in deps.RuntimeLibraries) {

				Assembly assembly = null;
				try {
					assembly = Assembly.Load(new AssemblyName(compilationLibrary.Name));
				} catch {
				}

				yield return assembly;
			}
		}
	}
}
