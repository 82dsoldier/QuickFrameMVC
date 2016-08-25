using Microsoft.AspNetCore.Mvc;
using QuickFrame.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Mvc.ViewComponents
{
    public class FileExplorerComponent : ViewComponent
    {
		public async Task<IViewComponentResult> InvokeAsync() {
			return await Task.FromResult<IViewComponentResult>(View(new ExplorerModel()));
		}
    }
}