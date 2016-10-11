using ExpressMapper;
using QuickFrame.Data.Attachments.Dtos;
using QuickFrame.Data.Attachments.Interfaces;
using QuickFrame.Data.Attachments.Models;
using QuickFrame.Data.Services;

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace QuickFrame.Data.Attachments.Services {

	[Export]
	public class UploadRulesDataService : DataService<AttachmentsContext, UploadRule>, IUploadRulesDataService {

		public UploadRulesDataService(AttachmentsContext context) : base(context) {
		}

		public int GetLastPriority() {
			return _dbContext.UploadRules
				.Where(obj => obj.Priority != Int32.MaxValue - 1)
				.OrderByDescending(obj => obj.Priority)
				.First()?.Priority ?? 1;
		}

		public IEnumerable<UploadRuleDto> GetUploadRules() {
			foreach(var obj in _dbContext.UploadRules.Where(obj => obj.IsActive == true && obj.IsDeleted == false))
				yield return Mapper.Map<UploadRule, UploadRuleDto>(obj);
		}
	}
}