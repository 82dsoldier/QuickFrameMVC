using System.Collections.Generic;

namespace QuickFrame.Security.AccountControl.Data.Models {

	public class SiteGroup {
		public string Id { get; set; }
		public string Name { get; set; }
		public List<string> Member { get; set; }
		public List<string> GroupType { get; set; }
		public string ObjectGuid { get; set; }
		public string AccountName { get; set; }
	}
}