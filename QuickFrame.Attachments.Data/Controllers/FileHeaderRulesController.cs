using QuickFrame.Attachments.Data.Dtos;
using QuickFrame.Attachments.Data.Models;
using QuickFrame.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuickFrame.Data.Interfaces;
using QuickFrame.Attachments.Data.Interfaces;

namespace QuickFrame.Attachments.Data.Controllers
{
	public class FileHeaderRulesController : ControllerBase<FileHeaderRule, FileHeaderRuleIndexDto, FileHeaderRuleEditDto> {
		public FileHeaderRulesController(IFileHeaderRulesDataService dataService) 
			: base(dataService) {
		}
	}
}