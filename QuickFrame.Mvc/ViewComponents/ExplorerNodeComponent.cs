using Microsoft.AspNetCore.Mvc;
using QuickFrame.Mvc.Interfaces;
using QuickFrame.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Mvc.ViewComponents {
	public class ExplorerNodeComponent : ViewComponent {
		public async Task<IViewComponentResult> InvokeAsync(ExplorerNode model) {
			return await Task.FromResult<IViewComponentResult>(View(model));
		}
	}
}