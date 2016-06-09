using QuickFrame.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace QuickFrame.Security.Data {

	public class UserBase : DataModel {

		/// <summary>
		/// Gets or sets the name of the user. Should be set to the active directory name.
		/// </summary>
		/// <value>The name of the user.</value>
		[Required]
		[StringLength(128)]
		public string UserId { get; set; }

		[Required]
		[StringLength(256)]
		public string DisplayName { get; set; }

		[Required]
		[StringLength(64)]
		public string FirstName { get; set; }

		[Required]
		[StringLength(128)]
		public string LastName { get; set; }

		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[DataType(DataType.PhoneNumber)]
		public string Phone { get; set; }

		[StringLength(1024)]
		public string Comments { get; set; }
	}
}