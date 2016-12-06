using QuickFrame.Data;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Security.Principal;

namespace QuickFrame.Security.AccountControl.ActiveDirectory.AdLookup {

	public static class AccountService {
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

		public static IEnumerable<Person> GetAccounts() {
			if(_searchRoot == null)
				throw new Exception("Invalid search path or search path not set");
			_searcher.Filter = $"(objectClass=person)";
			using(SearchResultCollection collection = _searcher.FindAll()) {
				for(int i = 0; i < collection.Count; i++) {
					if(collection[i].Properties["objectSid"].Count > 0)
						yield return new Person(collection[i]);
				}
			}
		}

		public static Person GetAccount(string accountId) {
			if(_searchRoot == null)
				throw new Exception("Invalid search path or search path not set");
			SecurityIdentifier sid = new SecurityIdentifier(accountId);
			_searcher.Filter = $"(&(objectClass=person)(objectSid={sid.ToHexString()}))";
			using(SearchResultCollection collection = _searcher.FindAll()) {
				if(collection != null && collection.Count > 0) {
					return new Person(collection[0]);
				}
			}
			return null;
		}

		public static Person GetAccountByUserName(string normalizedUserName) {
			if(_searchRoot == null)
				throw new Exception("Invalid search path or search path not set");
			_searcher.Filter = $"(&(objectClass=person)(sAMAccountName={normalizedUserName}))";
			using(SearchResultCollection collection = _searcher.FindAll()) {
				if(collection != null && collection.Count > 0) {
					return new Person(collection[0]);
				}
			}

			return null;
		}
	}
}