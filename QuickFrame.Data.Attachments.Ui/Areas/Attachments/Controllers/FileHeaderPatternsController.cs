using QuickFrame.Data.Attachments.Dtos;
using QuickFrame.Mvc;
using System;
using QuickFrame.Data.Attachments.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuickFrame.Data.Interfaces;
using QuickFrame.Data.Attachments.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace QuickFrame.Data.Attachments.Ui.Areas.Attachments.Controllers
{
	[Area("Attachments")]
	public class FileHeaderPatternsController : ControllerBase<FileHeaderPattern, FileHeaderPatternDto, FileHeaderPatternDto> {
		public FileHeaderPatternsController(IFileHeaderPatternsDataService dataService) : base(dataService) {
		}
	}
}

