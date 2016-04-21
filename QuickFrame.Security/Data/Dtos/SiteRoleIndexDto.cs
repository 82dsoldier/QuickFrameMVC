﻿using QuickFrame.Data;
using QuickFrame.Mapping;
using QuickFrame.Security.Data.Models;
using System.Collections.Generic;

namespace QuickFrame.Security.Data.Dtos {

	[ExpressMap]
	public class SiteRoleIndexDto : DataTransferObject<SiteRole, SiteRoleIndexDto> {
		public string Name { get; set; }
		public string Description { get; set; }
		public List<SiteUserIndexDto> Users { get; set; }
		public List<SiteGroupIndexDto> Groups { get; set; }
	}
}