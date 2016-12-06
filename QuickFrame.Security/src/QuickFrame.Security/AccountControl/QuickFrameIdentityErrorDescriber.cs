using Microsoft.AspNetCore.Identity;

namespace QuickFrame.Security.AccountControl {

	public class QuickFrameIdentityErrorDescriber : IdentityErrorDescriber {

		public virtual IdentityError GroupAlreadyInRole(string role) {
			return new IdentityError {
				Code = nameof(GroupAlreadyInRole),
				Description = $"Group already belogs to {role}"
			};
		}

		public virtual IdentityError GroupNotInRole(string role) {
			return new IdentityError {
				Code = nameof(GroupNotInRole),
				Description = $"Group not in {role}"
			};
		}
	}
}