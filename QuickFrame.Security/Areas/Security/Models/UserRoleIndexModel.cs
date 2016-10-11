using QuickFrame.Security.AccountControl.Dtos;
using System.Collections.Generic;

namespace QuickFrame.Security.Areas.Security.Models {

	public class UserRoleIndexModel {
		public string RoleId { get; set; }
		public string RoleName { get; set; }
		public List<UserRoleIndexDto> UserList { get; } = new List<UserRoleIndexDto>();
	}
}