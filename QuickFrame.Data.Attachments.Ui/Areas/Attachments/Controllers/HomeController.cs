using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Data.Attachments.Ui.Areas.Attachments.Controllers
{
	[Area("Attachments")]
    public class HomeController : Controller
    {
		public IActionResult Index() => RedirectToAction("Index", "Attachments");
    }
}
