using Microsoft.AspNetCore.Http;

namespace QuickFrame.Data.Attachments.Interfaces {

	public interface IAttachmentsSecurityService {

		bool CanFileUpload(IFormFile file);
	}
}