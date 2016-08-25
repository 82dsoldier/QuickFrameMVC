using Microsoft.AspNetCore.Mvc;
using QuickFrame.Data.Attachments.Dtos;
using QuickFrame.Data.Attachments.Interfaces;
using QuickFrame.Data.Attachments.Models;
using QuickFrame.Mvc;

namespace QuickFrame.Data.Attachments.Areas.Attachments.Controllers {

	[Area("Attachments")]
	public class FileHeaderPatternsController : ControllerCore<FileHeaderPattern, FileHeaderPatternDto, FileHeaderPatternDto> {

		public FileHeaderPatternsController(IFileHeaderPatternsDataService dataService) : base(dataService) {
		}
	}
}