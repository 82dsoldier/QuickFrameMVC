using QuickFrame.Security.AccountControl.ActiveDirectory.AdLookup.Interfaces;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Security.Claims;

namespace QuickFrame.Security.AccountControl.ActiveDirectory.AdLookup {

	public class Person {
		private IndexedProperty<string> _email;
		private IndexedProperty<byte[], string> _id;
		private IndexedProperty<string> _phoneNumber;
		private IndexedProperty<string> _userName;
		private IndexedProperty<string> _displayName;
		private Dictionary<string, string> _claims;
		private List<string> _groups;
		public IndexedProperty<string> Email { get { return _email; } }
		public string Id { get { return _id[0]; } }
		public IndexedProperty<string> PhoneNumber { get { return _phoneNumber; } }
		public string UserName { get { return _userName[0]; } }
		public Dictionary<string, string> Claims { get { return _claims; } }
		public List<string> Groups { get { return _groups; } }
		public string DisplayName { get { return _displayName.Count > 0 ? _displayName[0] : _userName[0]; } }

		public Person(SearchResult result) {
			//_searchRoot = searchRoot;
			_email = new IndexedProperty<string>(result.Properties["mail"]);
			_phoneNumber = new IndexedProperty<string>(result.Properties["phone"]);
			_id = new IndexedProperty<byte[], string>(result.Properties["objectSid"], new SidTransformer());
			_claims = new Dictionary<string, string>();
			_claims.Add(ClaimTypes.PrimarySid, Id);
			_userName = new IndexedProperty<string>(result.Properties["sAMAccountName"]);
			_displayName = new IndexedProperty<string>(result.Properties["displayName"]);
			if(result.Properties["departmenet"] != null && result.Properties["department"].Count > 0) {
				_groups = new List<string>();
				foreach(var department in result.Properties["department"]) {
					_claims.Add(ClaimTypes.GroupSid, department.ToString());
					_groups.Add(department.ToString());
				}
			}
		}
	}
}