using QuickFrame.Data.Attachments.Dtos;
using QuickFrame.Data.Attachments.Models;
using QuickFrame.Data.Interfaces;
using QuickFrame.Data.Interfaces.Services;
using System.Collections.Generic;

namespace QuickFrame.Data.Attachments.Interfaces {

	public interface IUploadRulesDataService : IDataService<UploadRule> {

		int GetLastPriority();

		IEnumerable<UploadRuleDto> GetUploadRules();
	}
}