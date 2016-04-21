﻿using QuickFrame.Data;
using QuickFrame.Mapping;
using QuickFrame.Security.Data.Models;
using System.Collections.Generic;

namespace QuickFrame.Security.Data.Dtos {

	[ExpressMap]
	public class SiteUserRoleCheckDto : DataTransferObject<SiteUser, SiteUserRoleCheckDto> {
		public string UserId { get; set; }
		public string DisplayName { get; set; }
		public List<SiteRoleIndexDto> Roles { get; set; }
	}
}