using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using QuickFrame.Data;
using QuickFrame.Mvc.Configuration;
using System;
using System.Collections.Generic;

namespace QuickFrame.Mvc.TagHelpers {

	[HtmlTargetElement("qf-paging", TagStructure = TagStructure.NormalOrSelfClosing)]
	public class PagingTagHelper : TagHelper {
		protected IHttpContextAccessor ContextAccessor;
		protected IHtmlGenerator Generator;
		protected ViewOptions ViewOptions;
		protected IUrlHelperFactory UrlHelperFactory;

		private PagingTagHelper(IHtmlGenerator generator, IOptions<ViewOptions> viewConfig, IHttpContextAccessor accessor, IUrlHelperFactory urlHelperFactory) {
			Generator = generator;
			ViewOptions = viewConfig.Value;
			ContextAccessor = accessor;
			UrlHelperFactory = urlHelperFactory;
		}

		[HtmlAttributeName("qf-items-per-page")]
		public int? ItemsPerPage { get; set; }

		[HtmlAttributeName("qf-total-items")]
		public int? TotalItems { get; set; }

		[HtmlAttributeName("qf-page")]
		public int? CurrentPage { get; set; }

		[HtmlAttributeName("qf-controller")]
		public string Controller { get; set; }

		[HtmlAttributeName("qf-action")]
		public string Action { get; set; }

		[HtmlAttributeName]
		[HtmlAttributeNotBound]
		[ViewContext]
		public ViewContext ViewContext { get; set; }

		public override void Process(TagHelperContext context, TagHelperOutput output) {
			var urlHelper = UrlHelperFactory.GetUrlHelper(
				new ActionContext(ContextAccessor.HttpContext,
				new RouteData(),
				new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()));

			var itemsPerPage = (ItemsPerPage ?? 0);

			if(itemsPerPage == 0)
				int.TryParse(ViewOptions.PerPageDefault, out itemsPerPage);

			var currentPage = (CurrentPage ?? 1);

			var totalItems = (TotalItems ?? 0);

			if(totalItems > itemsPerPage) {
				var totalPages = (totalItems + (itemsPerPage - 1)) / itemsPerPage;

				string controller = String.IsNullOrEmpty(Controller) ? (ViewContext.ActionDescriptor as ControllerActionDescriptor)?.ControllerName : Controller;
				string action = String.IsNullOrEmpty(Action) ? "Index" : Action;

				var pageList = new List<string> {
					"«",
					"‹"
				};

				switch(totalPages) {
					case 2:
						pageList.Add("1");
						pageList.Add("2");
						break;

					case 3:
					case 4:
					case 5:
						for(var i = 1; i < totalPages; i++)
							pageList.Add(i.ToString());
						break;

					default:
						if(currentPage < 4) {
							for(var i = 1; i < 6; i++)
								pageList.Add(i.ToString());
						} else {
							var endPage = currentPage + 2 < totalPages ? currentPage + 3 : totalPages + 1;
							for(var i = endPage - 5; i < endPage; i++)
								pageList.Add(i.ToString());
						}
						break;
				}

				pageList.Add("›");
				pageList.Add("»");

				var list = new FluentTagBuilder("ul").AddCssClass("pagination pagination-sm")
					.MergeAttribute("style", "display:inline;");

				foreach(var page in pageList) {
					object routeValues = null;

					if(page.IsNumeric())
						routeValues = new { page, itemsPerPage };
					else if(page == "«")
						routeValues = new { page = 1, itemsPerPage };
					else if(page == "‹")
						routeValues = new {
							page = currentPage - 1 > 0 ? currentPage - 1 : 1,
							itemsPerPage
						};
					else if(page == "›")
						routeValues = new {
							page = currentPage + 1 <= totalPages ? currentPage + 1 : totalPages,
							itemsPerPage
						};
					else if(page == "»")
						routeValues = new {
							page = totalPages,
							itemsPerPage
						};

					var li = new FluentTagBuilder("li")
						.AppendHtml(Generator.GenerateActionLink(ViewContext, page, action, controller, string.Empty, string.Empty, string.Empty, routeValues, null));

					if(page.IsNumeric()) {
						if(Convert.ToInt32(page) == currentPage)
							li.AddCssClass("active");
					}
					list.AppendHtml(li);
				}

				var perPageSelect = new FluentTagBuilder("select")
					.GenerateId("ddlResultsPerPage", "")
					.MergeAttribute("style", "width:auto;float:left;margin-right:15px;")
					.MergeAttribute("data-url", urlHelper.Action("Index", Controller))
					.AddCssClass("itemCountDropdown form-control")
					.MergeAttribute("onchange", "javascript:window.location.href = $('.itemCountDropdown option:selected').attr('tag')");

				foreach(var item in ViewOptions.PerPageList) {
					item.Selected = item.Value == itemsPerPage.ToString();
					var option = new FluentTagBuilder("option")
						.MergeAttribute("value", item.Value)
						.Append(item.Text)
						.MergeAttribute("tag", urlHelper.Action(action, controller, new { page = 1, itemsPerPage = item.Value }));

					if(item.Selected)
						option.MergeAttribute("selected", "selected");

					perPageSelect.AppendHtml(option);
				}

				output.PostElement.AppendHtml(perPageSelect);
				output.PostElement.AppendHtml(list);
			}
		}
	}
}