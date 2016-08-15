using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Security.Principal;

namespace QuickFrame.Security.AccountControl.ActiveDirectory.AdLookup {

	public static class GroupService {
		private static string _searchPath;
		private static DirectoryEntry _searchRoot;
		private static DirectorySearcher _searcher;

		public static string SearchPath
		{
			get { return _searchPath; }
			set
			{
				_searchPath = value;
				_searchRoot = new DirectoryEntry(_searchPath);
				_searcher = new DirectorySearcher(_searchRoot);
				_searcher.PropertiesToLoad.Add("objectSid");
				_searcher.PropertiesToLoad.Add("mail");
				_searcher.PropertiesToLoad.Add("sAMAccountName");
				_searcher.PropertiesToLoad.Add("displayName");
				_searcher.PropertiesToLoad.Add("department");
				_searcher.PropertiesToLoad.Add("telephoneNumber");
				_searcher.PageSize = 1000;
			}
		}

		public static IEnumerable<Group> GetGroups() {
			if(_searchRoot == null)
				throw new Exception("Invalid search path or search path not set");
			_searcher.Filter = $"(objectClass=group)";
			using(SearchResultCollection collection = _searcher.FindAll()) {
				for(int i = 0; i < collection.Count; i++) {
					if(collection[i].Properties["objectSid"].Count > 0)
						yield return new Group(collection[i]);
				}
			}
		}

		public static Group GetGroup(string groupId) {
			if(_searchRoot == null)
				throw new Exception("Invalid search path or search path not set");
			SecurityIdentifier sid = new SecurityIdentifier(groupId);
			_searcher.Filter = $"(&(objectClass=group)(objectSid={sid.ToHexString()}))";
			using(SearchResultCollection collection = _searcher.FindAll()) {
				if(collection != null && collection.Count > 0) {
					return new Group(collection[0]);
				}
			}
			return null;
		}

		public static Group GetGroupByGroupName(string normalizedUserName) {
			if(_searchRoot == null)
				throw new Exception("Invalid search path or search path not set");
			_searcher.Filter = $"(&(objectClass=group)(sAMAccountName={normalizedUserName}))";
			using(SearchResultCollection collection = _searcher.FindAll()) {
				if(collection != null && collection.Count > 0) {
					return new Group(collection[0]);
				}
			}

			return null;
		}
	}
}