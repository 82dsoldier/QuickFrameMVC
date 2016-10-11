using QuickFrame.Security.AccountControl.Dtos;
using System.Collections.Generic;

namespace QuickFrame.Security.Areas.Security.Models {

	public class GroupRoleIndexModel {
		public string RoleId { get; set; }
		public string RoleName { get; set; }
		public List<GroupRoleIndexDto> GroupList { get; } = new List<GroupRoleIndexDto>();
	}
}