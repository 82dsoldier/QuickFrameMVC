using Microsoft.AspNetCore.Http;

namespace QuickFrame.Data.Attachments.Interfaces {

	public interface IUploadRuleDto {

		bool IsMatch(IFormFile file);
	}
}