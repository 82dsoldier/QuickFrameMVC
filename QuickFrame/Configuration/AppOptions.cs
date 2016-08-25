using System.Collections.Generic;

namespace QuickFrame.Configuration {

	public class AppOptions {
		public List<string> LoadExcludeList { get; } = new List<string>();
		public bool SecurityIsLoaded { get; set; } = false;
	}
}