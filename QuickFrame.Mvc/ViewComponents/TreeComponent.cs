using Microsoft.AspNetCore.Mvc;
using QuickFrame.Mvc.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Mvc.ViewComponents
{
    public class TreeComponent : ViewComponent
    {
		public async Task<IViewComponentResult> InvokeAsync(ITreeModel model) {
			return await Task.Run<IViewComponentResult>(() => View(model));
		}
    }
}