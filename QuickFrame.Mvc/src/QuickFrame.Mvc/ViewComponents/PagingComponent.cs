using Microsoft.AspNetCore.Mvc;
using QuickFrame.Mvc.Models;
using System.Threading.Tasks;

namespace QuickFrame.Mvc.ViewComponents {

	[ViewComponent(Name = "Paging")]
	public class PagingComponent : ViewComponent {

		public Task<IViewComponentResult> InvokeAsync(PagingComponentModel model) {
			return Task.FromResult(View() as IViewComponentResult);
		}
	}
}