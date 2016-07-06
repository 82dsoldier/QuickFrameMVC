using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using QuickFrame.Configuration;
using System;
using System.Collections.Generic;

namespace QuickFrame.Mvc.Tags {

	/// <summary>
	/// Creates an HTML tag that is used to generate paging for a table that is larger than the specified number of elements.
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />
	[HtmlTargetElement("qf-paging", TagStructure = TagStructure.NormalOrSelfClosing)]
	public class PagingTagHelper : TagHelper {

		/// <summary>
		/// The context accessor
		/// </summary>
		protected IHttpContextAccessor ContextAccessor;

		/// <summary>
		/// The generator
		/// </summary>
		protected IHtmlGenerator Generator;

		/// <summary>
		/// The view options
		/// </summary>
		protected ViewOptions ViewOptions;
		protected IUrlHelperFactory UrlHelperFactory;
		/// <summary>
		/// Initializes a new instance of the <see cref="PagingTagHelper"/> class.
		/// </summary>
		/// <param name="generator">The generator.</param>
		/// <param name="viewConfig">The view configuration.</param>
		/// <param name="accessor">The accessor.</param>
		public PagingTagHelper(IHtmlGenerator generator, IOptions<ViewOptions> viewConfig, IHttpContextAccessor accessor, IUrlHelperFactory urlHelperFactory) {
			Generator = generator;
			ViewOptions = viewConfig.Value;
			ContextAccessor = accessor;
			UrlHelperFactory = urlHelperFactory;
		}

		/// <summary>
		/// Gets or sets the number of items to display on each page
		/// </summary>
		/// <value>
		/// The items per page.
		/// </value>
		[HtmlAttributeName("qf-items-per-page")]
		public object ItemsPerPage { get; set; }

		/// <summary>
		/// Gets or sets the total items being displayed
		/// </summary>
		/// <value>
		/// The total items.
		/// </value>
		[HtmlAttributeName("qf-total-items")]
		public object TotalItems { get; set; }

		/// <summary>
		/// Gets or sets the current page.
		/// </summary>
		/// <value>
		/// The current page.
		/// </value>
		[HtmlAttributeName("qf-page")]
		public object CurrentPage { get; set; }

		/// <summary>
		/// Gets or sets the controller to use when creating paging hyperlinks.
		/// </summary>
		/// <value>
		/// The controller.
		/// </value>
		[HtmlAttributeName("qf-controller")]
		public string Controller { get; set; }

		/// <summary>
		/// Gets or sets the action to use when creating paging hyperlinks.
		/// </summary>
		/// <value>
		/// The action.
		/// </value>
		[HtmlAttributeName("qf-action")]
		public string Action { get; set; }

		/// <summary>
		/// Gets or sets the view context.
		/// </summary>
		/// <value>
		/// The view context.
		/// </value>
		[HtmlAttributeName]
		[HtmlAttributeNotBound]
		[ViewContext]
		public ViewContext ViewContext { get; set; }

		/// <summary>
		/// Synchronously executes the <see cref="T:Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" /> with the given <paramref name="context" /> and
		/// <paramref name="output" />.
		/// </summary>
		/// <param name="context">Contains information associated with the current HTML tag.</param>
		/// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
		public override void Process(TagHelperContext context, TagHelperOutput output) {
			var urlHelper = UrlHelperFactory.GetUrlHelper(new ActionContext(ContextAccessor.HttpContext, new Microsoft.AspNetCore.Routing.RouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor())); //ContextAccessor.HttpContext.RequestServices.GetRequiredService<IUrlHelper>();

			var itemsPerPage = 0;

			if(ItemsPerPage != null)
				int.TryParse(ItemsPerPage.ToString(), out itemsPerPage);

			if(itemsPerPage == 0)
				int.TryParse(ViewOptions.PerPageDefault, out itemsPerPage);

			var currentPage = 1;

			int.TryParse(CurrentPage?.ToString(), out currentPage);

			var totalItems = 0;

			int.TryParse(TotalItems?.ToString(), out totalItems);

			if(totalItems > itemsPerPage) {
				//TODO:  Check for zero itemsPerPage.  Shouldn't ever happen unless the appsettings.json is screwed up
				var totalPages = (totalItems + (itemsPerPage - 1)) / itemsPerPage;

				if(string.IsNullOrEmpty(Controller))
					Controller = (ViewContext.ActionDescriptor as ControllerActionDescriptor)?.ControllerName;

				if(string.IsNullOrEmpty(Action))
					Action = "Index";

				var pageList = new List<string>();
				pageList.Add("«");
				pageList.Add("‹");
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

				var list = new FluentTagBuilder("ul")
					.AddCssClass("pagination pagination-sm")
					.MergeAttribute("style", "display:inline;");

				foreach(var page in pageList) {
					object routeValues = null;
					if(page.IsNumeric())
						routeValues = new { page, itemsPerPage };
					else if(page == "«")
						routeValues = new { page = 1, itemsPerPage };
					else if(page == "‹")
						routeValues = new { page = currentPage - 1 > 0 ? currentPage - 1 : 1, itemsPerPage };
					else if(page == "›")
						routeValues = new { page = currentPage + 1 <= totalPages ? currentPage + 1 : totalPages, itemsPerPage };
					else if(page == "»")
						routeValues = new { page = totalPages, itemsPerPage };

					var li = new FluentTagBuilder("li")
						.AppendHtml(Generator.GenerateActionLink(ViewContext,
							page,
							Action,
							Controller,
							string.Empty,
							string.Empty,
							string.Empty,
							routeValues, null));

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
						.MergeAttribute("tag", urlHelper.Action(Action, Controller, new { page = 1, itemsPerPage = item.Value }));

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