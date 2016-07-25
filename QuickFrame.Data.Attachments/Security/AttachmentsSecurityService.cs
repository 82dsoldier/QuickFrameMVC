using Microsoft.AspNetCore.Http;
using QuickFrame.Data.Attachments.Dtos;
using QuickFrame.Data.Attachments.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Data.Attachments.Security
{
    public class AttachmentsSecurityService
    {
		private IUploadRulesDataService _uploadRulesDataService;

		public bool CanFileUpload(IFormFile file) {
			List<UploadRuleDto> rules = _uploadRulesDataService.GetUploadRules().ToList();
			foreach(var rule in rules)
				if(!rule.CanFileUpload(file))
					return false;

			return true;
		}

		public AttachmentsSecurityService(IUploadRulesDataService uploadRulesDataService) {
			_uploadRulesDataService = uploadRulesDataService;
		}
	}
}
