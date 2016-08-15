using Microsoft.AspNetCore.Mvc;

namespace QuickFrame.Data.Attachments.Ui.Areas.Attachments.Controllers {

	[Area("Attachments")]
	public class HomeController : Controller {

		public IActionResult Index() => RedirectToAction("Index", "Attachments");
	}
}