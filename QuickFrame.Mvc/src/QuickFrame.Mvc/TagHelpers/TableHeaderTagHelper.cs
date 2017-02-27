using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using QuickFrame.Mvc.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;

namespace QuickFrame.Mvc.TagHelpers {

	/// <summary>
	/// Pulls the name from the specified property and displays it in the th element
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Razor.TagHelpers.TagHelper"/>
	[HtmlTargetElement("th", Attributes = "display-for")]
	public class TableHeaderTagHelper : TagHelper {

		/// <summary>
		/// The name of the attribute used to provide the element being displayed
		/// </summary>
		private const string DisplayForAttributeName = "display-for";

		/// <summary>
		/// Allows access to the current context.
		/// </summary>
		/// <seealso cref="Microsoft.AspNetCore.Http.IHttpContextAccessor"/>
		protected IHttpContextAccessor ContextAccessor;

		/// <summary>
		/// Used to generate certain HTML tags.
		/// </summary>
		/// <seealso cref="Microsoft.AspNetCore.Mvc.ViewFeatures.IHtmlGenerator"/>
		protected IHtmlGenerator Generator;

		/// <summary>
		/// Provides access to the view options provided by the application.
		/// </summary>
		protected ViewOptions ViewOptions;

		/// <summary>
		/// Gets or sets the metadata provider.
		/// </summary>
		/// <value>The metadata provider.</value>
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


		[HtmlAttributeName(DisplayForAttributeName)]
		public string DisplayFor { get; set; }

		[HtmlAttributeName("qf-sort")]
		public bool Sort { get; set; }

		[HtmlAttributeName("qf-controller")]
		public string Controller { get; set; }

		[HtmlAttributeName("qf-action")]
		public string Action { get; set; }

		[HtmlAttributeNotBound]
		[ViewContext]
		public ViewContext ViewContext { get; set; }

		public override void Process(TagHelperContext context, TagHelperOutput output) {
			var modelMetadata = ViewContext.ViewData.ModelMetadata;

			var props = new List<ModelMetadata>(); //			GetProperties(modelMetadata);
			props.AddRange(MetadataProvider.GetMetadataForProperties(modelMetadata.ElementType ?? modelMetadata.ModelType));

			var columnName = string.Empty;

			if(!DisplayFor.Contains(".")) {
				var chosen = props.SingleOrDefault(p => p.PropertyName == DisplayFor);
				if(chosen != null) {
					output.Content.SetContent(chosen.GetDisplayName());
					columnName = chosen.PropertyName;
				}
			} else {
				var propertyNames = DisplayFor.Split('.');
				foreach(var propertyName in propertyNames) {
					modelMetadata = props.SingleOrDefault(p => p.PropertyName == propertyName);
					if(modelMetadata == null)
						break;
					if(propertyName == propertyNames[propertyNames.Length - 1]) {
						var displayName = modelMetadata.GetDisplayName();
						if(String.IsNullOrEmpty(displayName))
							displayName = propertyName;
						output.Content.SetContent(displayName);
						columnName = modelMetadata.PropertyName;
					} else {
						props.Clear();
						props.AddRange(MetadataProvider.GetMetadataForProperties(modelMetadata.ElementType));
					}
				}
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

				var controller = String.IsNullOrEmpty(Controller) ? (ViewContext.ActionDescriptor as ControllerActionDescriptor)?.ControllerName : Controller;

				var action = String.IsNullOrEmpty(Action) ? ViewContext.RouteData.Values["action"].ToString() : Action;
				if(string.IsNullOrEmpty(action))
					action = "Index";

				dynamic routeValues = new ExpandoObject();
				foreach(var obj in ContextAccessor.HttpContext.Request.Query)
					((System.Collections.Generic.IDictionary<string, object>)routeValues)[obj.Key] = obj.Value;

				routeValues.sortColumn = columnName;
				routeValues.sortOrder = SortOrder.Ascending;

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
						routeValues,
						new { @class = "fa fa-sort-asc" })));
				routeValues.sortOrder = SortOrder.Descending;
				ul.AppendHtml(new FluentTagBuilder("li")
						.AppendHtml(Generator.GenerateActionLink(ViewContext,
						string.Empty,
						action,
						controller,
						string.Empty,
						string.Empty,
						string.Empty,
						routeValues,
						new { @class = "fa fa-sort-desc" })));

				output.Content.AppendHtml(ul);
			}
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