using Microsoft.AspNetCore.Mvc;
using QuickFrame.Data.Attachments.Dtos;
using QuickFrame.Data.Attachments.Interfaces;
using QuickFrame.Data.Attachments.Models;
using QuickFrame.Mvc;

namespace QuickFrame.Data.Attachments.Ui.Areas.Attachments.Controllers {

	[Area("Attachments")]
	public class FileExtensionsController : ControllerCore<FileExtension, FileExtensionDto, FileExtensionDto> {

		public FileExtensionsController(IFileExtensionsDataService dataService) : base(dataService) {
		}
	}
}