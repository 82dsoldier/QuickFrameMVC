using Microsoft.AspNetCore.Mvc;

namespace QuickFrame.Mvc.Controllers {

	public class StatusCodes : Controller {

		public IActionResult StatusCode403() {
			return View();
		}

		public IActionResult StatusCode401() {
			return View();
		}

		public IActionResult StatusCode404() {
			return View();
		}
	}
}