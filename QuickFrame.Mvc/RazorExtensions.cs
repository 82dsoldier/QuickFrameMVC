using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using QuickFrame.Di;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Mvc
{
    public static class RazorExtensions
    {
		public static bool ViewExists(string viewName, bool isMainPage = true) {
			var viewEngine = ComponentContainer.Component<ICompositeViewEngine>();
			var contextAccessor = ComponentContainer.Component<IActionContextAccessor>();
			ViewEngineResult result = viewEngine.Component.FindView(contextAccessor.Component.ActionContext, viewName, isMainPage);
			return result.View != null;
		}
    }
}
