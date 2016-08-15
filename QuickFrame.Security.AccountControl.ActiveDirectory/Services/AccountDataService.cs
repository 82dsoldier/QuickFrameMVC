using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using QuickFrame.Security.AccountControl.Data.Models;
using QuickFrame.Security.AccountControl.Interfaces;
using QuickFrame.Security.Configuration;
using System;
using System.Collections.Generic;
using System.Composition;
using System.DirectoryServices;
using System.Security.Principal;

namespace QuickFrame.Security.AccountControl.ActiveDirectory.Services {

	[Export(typeof(IAccountDataService))]
	public class AccountDataService : IAccountDataService {
		private NameListOptions _excludedNameOptions;

		public IEnumerable<TAccount> GetUsers<TAccount>(string filter = "") where TAccount : IdentityUser => GetUsersBase(filter)/*.OrderBy(u => u.DisplayName)*/ as IEnumerable<TAccount>;

		public TAccount GetUser<TAccount>(string userId) where TAccount : IdentityUser => GetUser(userId) as TAccount;

		public string BuildFilter(List<IdentityUser> inclusions = null, List<IdentityUser> exclusions = null) {
			string filter = String.Empty;
			//if(inclusions != null) {
			//	foreach(var inclusion in inclusions) {
			//		if(!String.IsNullOrEmpty(filter)) {
			//			filter = $"{filter}(|";
			//		} else {
			//			filter = "(";
			//		}
			//		if(!String.IsNullOrEmpty(inclusion.UserId)) {
			//			SecurityIdentifier sid = new SecurityIdentifier(inclusion.UserId);
			//			filter = $"{filter}(objectSid={sid.ToHexString()})";
			//		}
			//		if(!String.IsNullOrEmpty(inclusion.DisplayName)) {
			//			filter = $"{filter}(displayName={inclusion.DisplayName})";
			//		}
			//		if(!String.IsNullOrEmpty(inclusion.FirstName)) {
			//			filter = $"{filter}(givenName={inclusion.FirstName})";
			//		}
			//		if(!String.IsNullOrEmpty(inclusion.LastName)) {
			//			filter = $"{filter}(sn={inclusion.LastName})";
			//		}
			//		if(!String.IsNullOrEmpty(inclusion.Email)) {
			//			filter = $"{filter}(mail={inclusion.Email})";
			//		}
			//		if(!String.IsNullOrEmpty(inclusion.Phone)) {
			//			filter = $"{filter}(telephoneNumber={inclusion.Phone})";
			//		}
			//		filter = $"{filter})";
			//	}
			//}

			//if(exclusions != null) {
			//	foreach(var exclusion in exclusions) {
			//		if(!String.IsNullOrEmpty(filter)) {
			//			filter = $"{filter}(|";
			//		} else {
			//			filter = "(";
			//		}
			//		if(!String.IsNullOrEmpty(exclusion.UserId)) {
			//			SecurityIdentifier sid = new SecurityIdentifier(exclusion.UserId);
			//			filter = $"{filter}(!(objectSid={sid.ToHexString()}))";
			//		}
			//		if(!String.IsNullOrEmpty(exclusion.DisplayName)) {
			//			filter = $"{filter}(!(displayName={exclusion.DisplayName}))";
			//		}
			//		if(!String.IsNullOrEmpty(exclusion.FirstName)) {
			//			filter = $"{filter}(!(givenName={exclusion.FirstName}))";
			//		}
			//		if(!String.IsNullOrEmpty(exclusion.LastName)) {
			//			filter = $"{filter}(!(sn={exclusion.LastName}))";
			//		}
			//		if(!String.IsNullOrEmpty(exclusion.Email)) {
			//			filter = $"{filter}(!(mail={exclusion.Email}))";
			//		}
			//		if(!String.IsNullOrEmpty(exclusion.Phone)) {
			//			filter = $"{filter}(!(telephoneNumber={exclusion.Phone}))";
			//		}
			//		filter = $"{filter})";
			//	}
			//}
			return filter;
		}

		public AccountDataService(IOptions<NameListOptions> options) {
			_excludedNameOptions = options.Value;
		}

		private IEnumerable<IdentityUser> GetUsersBase(string filter = "") {
			DirectoryEntry searchRoot = new DirectoryEntry("LDAP://DC=DEAC,DC=PAD,DC=LOCAL");
			using(DirectorySearcher searcher = new DirectorySearcher(searchRoot)) {
				searcher.PageSize = 1000;
				searcher.Filter = $"(&(objectClass=user)(objectCategory=person){filter})";
				searcher.PropertiesToLoad.Add("mail");
				searcher.PropertiesToLoad.Add("objectSid");
				searcher.PropertiesToLoad.Add("displayName");
				searcher.PropertiesToLoad.Add("givenName");
				searcher.PropertiesToLoad.Add("sn");
				searcher.PropertiesToLoad.Add("telephoneNumber");
				searcher.PropertiesToLoad.Add("sAmAccountName");
				searcher.PropertiesToLoad.Add("userAccountControl");
				using(SearchResultCollection searchCollection = searcher.FindAll()) {
					if(searchCollection != null) {
						for(int ct = 0; ct < searchCollection.Count; ct++) {
							SearchResult result = searchCollection[ct];
							var flags = Convert.ToInt32(result.Properties["userAccountControl"][0]);
							if((flags & 2) > 0) {
								if(result.Properties["displayName"].Count > 0 && !String.IsNullOrEmpty(result.Properties["displayName"][0].ToString())) {
									if(_excludedNameOptions.IsValid(result.Properties["displayName"][0].ToString())) {
										yield return new SiteUser {
											//UserId = new SecurityIdentifier((byte[])result.Properties["objectSid"][0], 0).Value,
											//DisplayName = result.Properties["displayName"].Count > 0 ? result.Properties["displayName"][0].ToString() : "",
											//Email = result.Properties["mail"].Count > 0 ? result.Properties["mail"][0].ToString() : "",
											//FirstName = result.Properties["givenName"].Count > 0 ? result.Properties["givenName"][0].ToString() : "",
											//LastName = result.Properties["sn"].Count > 0 ? result.Properties["sn"][0].ToString() : "",
											//Phone = result.Properties["telephoneNumber"].Count > 0 ? result.Properties["telephoneNumber"][0].ToString() : "",
										};
									}
								}
							}
						}
					}
				}
			}
		}

		private SiteUser GetUser(string userId) {
			SecurityIdentifier sid = new SecurityIdentifier(userId);
			DirectoryEntry searchRoot = new DirectoryEntry("LDAP://DC=DEAC,DC=PAD,DC=LOCAL");
			using(DirectorySearcher searcher = new DirectorySearcher(searchRoot)) {
				searcher.PageSize = 1000;
				searcher.Filter = $"(&(objectClass=user)(objectCategory=person)(objectSid={sid.ToHexString()}))";
				searcher.PropertiesToLoad.Add("mail");
				searcher.PropertiesToLoad.Add("objectSid");
				searcher.PropertiesToLoad.Add("displayName");
				searcher.PropertiesToLoad.Add("givenName");
				searcher.PropertiesToLoad.Add("sn");
				searcher.PropertiesToLoad.Add("telephoneNumber");
				searcher.PropertiesToLoad.Add("sAmAccountName");
				searcher.PropertiesToLoad.Add("userAccountControl");
				using(SearchResultCollection searchCollection = searcher.FindAll()) {
					if(searchCollection != null) {
						SearchResult result = searchCollection[0];
						var flags = Convert.ToInt32(result.Properties["userAccountControl"][0]);
						if((flags & 2) > 0) {
							if(result.Properties["displayName"].Count > 0 && !String.IsNullOrEmpty(result.Properties["displayName"][0].ToString())) {
								if(_excludedNameOptions.IsValid(result.Properties["displayName"][0].ToString())) {
									return new SiteUser {
										//UserId = new SecurityIdentifier((byte[])result.Properties["objectSid"][0], 0).Value,
										//DisplayName = result.Properties["displayName"].Count > 0 ? result.Properties["displayName"][0].ToString() : "",
										//Email = result.Properties["mail"].Count > 0 ? result.Properties["mail"][0].ToString() : "",
										//FirstName = result.Properties["givenName"].Count > 0 ? result.Properties["givenName"][0].ToString() : "",
										//LastName = result.Properties["sn"].Count > 0 ? result.Properties["sn"][0].ToString() : "",
										//Phone = result.Properties["telephoneNumber"].Count > 0 ? result.Properties["telephoneNumber"][0].ToString() : "",
									};
								}
							}
						}
					}
				}
			}

			return null;
		}

		private bool IsSystemAccount(string accountName) {
			if(accountName.Equals("sqlrpt", StringComparison.CurrentCultureIgnoreCase))
				return true;
			return false;
		}
	}
}