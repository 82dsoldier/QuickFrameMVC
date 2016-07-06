using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using QuickFrame.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace QuickFrame.Mvc.Tags {

	/// <summary>
	/// Pulls the name from the specified property and displays it in the th element
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />
	[HtmlTargetElement("th", Attributes = "display-for")]
	public class TableHeaderTagHelper : TagHelper {

		/// <summary>
		/// The name of the attribute used to provide the element being displayed
		/// </summary>
		private const string DisplayForAttributeName = "display-for";

		/// <summary>
		/// Allows access to the current context.
		/// </summary>
		/// <seealso cref="Microsoft.AspNetCore.Http.IHttpContextAccessor" />
		protected IHttpContextAccessor ContextAccessor;

		/// <summary>
		/// Used to generate certain HTML tags.
		/// </summary>
		/// <seealso cref="Microsoft.AspNetCore.Mvc.ViewFeatures.IHtmlGenerator" />
		protected IHtmlGenerator Generator;

		/// <summary>
		/// Provides access to the view options provided by the application.
		/// </summary>
		protected ViewOptions ViewOptions;

		/// <summary>
		/// Gets or sets the metadata provider.
		/// </summary>
		/// <value>
		/// The metadata provider.
		/// </value>
		protected internal IModelMetadataProvider MetadataProvider { get; set; }
		protected IUrlHelperFactory UrlHelperFactory;

		/// <summary>
		/// Initializes a new instance of the <see cref="ColumnHeaderTagHelper"/> class.
		/// </summary>
		/// <param name="metadataProvider">The metadata provider.</param>
		/// <param name="generator">The generator.</param>
		/// <param name="accessor">The accessor.</param>
		/// <param name="viewConfig">The view configuration.</param>
		public TableHeaderTagHelper(IModelMetadataProvider metadataProvider, IHtmlGenerator generator,
			IHttpContextAccessor accessor, IOptions<ViewOptions> viewConfig, IUrlHelperFactory urlHelperFactory) {
			MetadataProvider = metadataProvider;
			ContextAccessor = accessor;
			Generator = generator;
			ViewOptions = viewConfig.Value;
			UrlHelperFactory = urlHelperFactory;
		}

		/// <summary>
		/// Gets or sets the object passed into the display-for attribute.
		/// </summary>
		/// <value>
		/// The value passed into the display-for attribute.
		/// </value>
		[HtmlAttributeName(DisplayForAttributeName)]
		public string DisplayFor { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="ColumnHeaderTagHelper"/> is sortable.
		/// </summary>
		/// <value>
		///   <c>true</c> if sorting is to be used; otherwise, <c>false</c>.
		/// </value>
		[HtmlAttributeName("qf-sort")]
		public bool Sort { get; set; }

		//[HtmlAttributeName("qf-filter")]
		//public FilterStyle FilterStyle { get; set; }

		//[HtmlAttributeName("qf-data-url")]
		//public string DataUrl { get; set; }
		//[HtmlAttributeName("qf-column")]
		//public string Column { get; set; }
		//[HtmlAttributeName("qf-search-column")]
		//public string SearchColumn { get; set; }
		//[HtmlAttributeName("qf-function")]
		//public string SearchFunction { get; set; }
		/// <summary>
		/// Gets or sets the view context used to access the view model for the page containing this tag.
		/// </summary>
		/// <value>
		/// The view context.
		/// </value>
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
			if(string.IsNullOrEmpty(DisplayFor))
				return;

			var modelMetadata = ViewContext.ViewData.ModelMetadata;

			var props = GetProperties(modelMetadata);
			var columnName = string.Empty;

			if(!DisplayFor.Contains(".")) {
				var chosen = props.SingleOrDefault(p => p.PropertyName == DisplayFor);
				if(chosen != null) {
					output.Content.SetContent(chosen.GetDisplayName());
					columnName = chosen.PropertyName;
				}
			} else {
				ModelMetadata chosen = null;
				var displayNames = DisplayFor.Split('.');
				foreach(var displayName in displayNames) {
					chosen = props.SingleOrDefault(p => p.PropertyName == displayName);
					if(chosen == null) {
						return;
					}
					props = GetProperties(chosen);
				}
				output.Content.SetContent(chosen.GetDisplayName());
				columnName = chosen.PropertyName;
			}

			if(Sort) {
				var urlHelper = UrlHelperFactory.GetUrlHelper(new ActionContext(ContextAccessor.HttpContext, new Microsoft.AspNetCore.Routing.RouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor())); //ContextAccessor.HttpContext.RequestServices.GetRequiredService<IUrlHelper>();

				var itemsPerPage = 0;
				int.TryParse(ContextAccessor.HttpContext.Request.Query["itemsPerPage"], out itemsPerPage);

				if(itemsPerPage == 0)
					int.TryParse(ViewOptions.PerPageDefault, out itemsPerPage);

				var currentPage = 1;
				int.TryParse(ContextAccessor.HttpContext.Request.Query["page"], out currentPage);
				if(currentPage == 0)
					currentPage = 1;

				var controller = (ViewContext.ActionDescriptor as ControllerActionDescriptor)?.ControllerName;

				var action = "Index";

				var ul = new FluentTagBuilder("ul")
					.AddCssClass("sort-spinner")
					.AppendHtml(new FluentTagBuilder("li")
						.AppendHtml(Generator.GenerateActionLink(ViewContext,
						string.Empty,
						action,
						controller,
						string.Empty,
						string.Empty,
						string.Empty,
						new { page = currentPage, itemsPerPage, sortColumn = columnName },
						new { @class = "fa fa-sort-asc" })))
					.AppendHtml(new FluentTagBuilder("li")
						.AppendHtml(Generator.GenerateActionLink(ViewContext,
						string.Empty,
						action,
						controller,
						string.Empty,
						string.Empty,
						string.Empty,
						new { page = currentPage, itemsPerPage, sortColumn = columnName, sortOrder = SortOrder.Descending },
						new { @class = "fa fa-sort-desc" })));

				output.Content.AppendHtml(ul);
			}
			
			//if(FilterStyle != FilterStyle.None) {
			//	var filterLink = new FluentTagBuilder("i")
			//		.AddCssClass("fa fa-search reveal")
			//		.MergeAttribute("style", "cursor:pointer;margin-left:15px;vertical-align:middle;")
			//		.AppendHtml(new FluentTagBuilder("br"));

			//	output.Content.AppendHtml(filterLink);

			//	var displayType = (SearchColumn == Column) ? "block" : "none";

			//	var span = new FluentTagBuilder("span")
			//		.MergeAttribute("style", $"display:{displayType}")
			//		.AddCssClass("search-box");

			//	if(FilterStyle == FilterStyle.TextBox) {
			//		var textBox = new FluentTagBuilder("input")
			//			.MergeAttributes(new Dictionary<string, string> {
			//				{ "type", "text" },
			//				{"placeholder", "Search" },
			//				{"qf-column", Column },
			//				{"onSearch", SearchFunction }
			//				})
			//			.AddCssClass("form-control");
			//		output.Content.AppendHtml(span.AppendHtml(textBox));
			//	}
			//	if(FilterStyle == FilterStyle.Dropdown) {
			//		var dropdown = new FluentTagBuilder("select")
			//			.AddCssClass("form-control")
			//			.MergeAttributes(new Dictionary<string, string> {
			//				{"dc-dropdown-box", "" },
			//				{"dc-data-url", DataUrl },
			//				{"qf-column", Column },
			//				{"onSearch", SearchFunction }
			//			});
			//		output.Content.AppendHtml(span.AppendHtml(dropdown));
			//	}
			//}
		}

		/// <summary>
		/// Gets the properties of the specified model metadata
		/// </summary>
		/// <param name="parent">The parent.</param>
		/// <returns></returns>
		private List<ModelMetadata> GetProperties(ModelMetadata parent) {
			var props = new List<ModelMetadata>();
			var elementTypes = parent.ModelType.GetGenericArguments();
			if(elementTypes.Any()) {
				foreach(var elementType in elementTypes) {
					props.AddRange(MetadataProvider.GetMetadataForProperties(elementType));
				}
			} else {
				props.AddRange(MetadataProvider.GetMetadataForProperties(parent.ModelType));
			}

			return props;
		}

		/// <summary>
		/// Gets the metadata property
		/// </summary>
		/// <param name="metadata">The metadata.</param>
		/// <param name="propertyName">Name of the property.</param>
		/// <returns></returns>
		private ModelMetadata GetMetadata(ModelMetadata metadata, string propertyName) {
			return metadata.ModelType.GetGenericArguments().Select(elementType => MetadataProvider.GetMetadataForProperties(elementType)).Select(props => props.SingleOrDefault(p => p.PropertyName == propertyName)).FirstOrDefault(chosen => chosen != null);
		}
	}
}