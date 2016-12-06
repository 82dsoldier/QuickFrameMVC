using System.ComponentModel.DataAnnotations;

namespace QuickFrame.Security.AccountControl.Dtos {

	public class UserRoleIndexDto {
		public string RoleId { get; set; }
		public string RoleName { get; set; }
		public string UserId { get; set; }
		public string UserName { get; set; }

		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[DataType(DataType.PhoneNumber)]
		public string Phone { get; set; }
	}
}