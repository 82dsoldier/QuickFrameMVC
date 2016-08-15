using ExpressMapper;
using QuickFrame.Data.Attachments.Dtos;
using QuickFrame.Data.Attachments.Interfaces;
using QuickFrame.Data.Attachments.Models;
using QuickFrame.Data.Services;
using QuickFrame.Di;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace QuickFrame.Data.Attachments.Services {

	[Export]
	public class UploadRulesDataService : DataService<AttachmentsContext, UploadRule>, IUploadRulesDataService {

		public int GetLastPriority() {
			using(var context = ComponentContainer.Component<AttachmentsContext>()) {
				return context.Component.UploadRules
					.Where(obj => obj.Priority != Int32.MaxValue - 1)
					.OrderByDescending(obj => obj.Priority)
					.First()?.Priority ?? 1;
			}
		}

		public IEnumerable<UploadRuleDto> GetUploadRules() {
			using(var context = ComponentContainer.Component<AttachmentsContext>()) {
				foreach(var obj in context.Component.UploadRules.Where(obj => obj.IsActive == true && obj.IsDeleted == false))
					yield return Mapper.Map<UploadRule, UploadRuleDto>(obj);
			}
		}
	}
}