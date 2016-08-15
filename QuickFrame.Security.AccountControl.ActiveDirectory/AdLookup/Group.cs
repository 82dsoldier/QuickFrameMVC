﻿using ExpressMapper;
using QuickFrame.Data.Dtos;
using QuickFrame.Mapping;
using QuickFrame.Security.AccountControl.ActiveDirectory.AdLookup.Interfaces;
using QuickFrame.Security.AccountControl.Data.Models;
using System.DirectoryServices;

namespace QuickFrame.Security.AccountControl.ActiveDirectory.AdLookup {

	[ExpressMap]
	public class Group : GenericDataTransferObject<Group, SiteGroup> {
		private string _commonName;
		private IndexedProperty<string> _member;
		private IndexedProperty<int[], string[]> _groupType;
		private IndexedProperty<string> _memberOf;
		private IndexedProperty<byte[], string> _objectGuid;
		private IndexedProperty<byte[], string> _objectSid;
		private IndexedProperty<string> _accountName;
		public string CommonName { get { return _commonName; } }
		public IndexedProperty<string> Member { get { return _member; } }
		public IndexedProperty<int[], string[]> GroupType { get { return _groupType; } }
		public IndexedProperty<string> MemberOf { get { return _memberOf; } }
		public string ObjectGuid { get { return _objectGuid[0]; } }
		public string ObjectSid { get { return _objectSid[0]; } }
		public string AccountName { get { return _accountName[0]; } }

		public Group(SearchResult result) {
			_commonName = result.Properties["cn"][0].ToString();
			_member = new IndexedProperty<string>(result.Properties["member"]);
			_groupType = new IndexedProperty<int[], string[]>(result.Properties["groupType"], new GroupTypeTransformer());
			_memberOf = new IndexedProperty<string>(result.Properties["memberOf"]);
			_objectGuid = new IndexedProperty<byte[], string>(result.Properties["objectGuid"], new GuidTransformer());
			_objectSid = new IndexedProperty<byte[], string>(result.Properties["objectSid"], new SidTransformer());
			_accountName = new IndexedProperty<string>(result.Properties["sAMAccountName"]);
		}

		public override void Register() {
			Mapper.Register<Group, SiteGroup>()
				.Member(dest => dest.Id, src => src.ObjectSid)
				.Member(dest => dest.Name, src => src.CommonName);
		}
	}
}