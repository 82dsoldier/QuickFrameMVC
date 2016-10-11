using Microsoft.AspNetCore.Mvc;
using QuickFrame.Data.Attachments.Dtos;
using QuickFrame.Data.Attachments.Interfaces;
using QuickFrame.Data.Attachments.Models;
using QuickFrame.Mvc;
using QuickFrame.Mvc.Controllers;
using QuickFrame.Security;

namespace QuickFrame.Data.Attachments.Areas.Attachments.Controllers {

	[Area("Attachments")]
	public class FileHeaderPatternsController : QfController<FileHeaderPattern, FileHeaderPatternDto, FileHeaderPatternDto> {

		public FileHeaderPatternsController(IFileHeaderPatternsDataService dataService, QuickFrameSecurityManager securityManager) : base(dataService, securityManager) {
		}
	}
}