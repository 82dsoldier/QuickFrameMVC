using System.Collections.Generic;

namespace QuickFrame.Security.AccountControl.Models {

	public class SiteGroup {
		public string Id { get; set; }
		public string Name { get; set; }
		public List<string> Member { get; set; } = new List<string>();
		public List<string> GroupType { get; set; } = new List<string>();
		public string ObjectGuid { get; set; }
		public string AccountName { get; set; }
	}
}