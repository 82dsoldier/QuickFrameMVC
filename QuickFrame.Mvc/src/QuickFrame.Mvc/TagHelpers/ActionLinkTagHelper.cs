using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.WebEncoders;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Mvc.TagHelpers
{
	[HtmlTargetElement("action-link", TagStructure = TagStructure.NormalOrSelfClosing)]
	public class ActionLinkTagHelper : TagHelper
    {
		private IDictionary<string, string> _routeValues;
		private IDictionary<string, string> _fbValues;

		private IHttpContextAccessor _contextAccessor;
		private IHtmlGenerator _generator;
		private IUrlHelperFactory _urlHelperFactory;
		protected string caption { get; set; }

		[ViewContext]
		public ViewContext ViewContext { get; set; }
		[HtmlAttributeName("asp-icon")]
		public string Icon { get; set; }
		[HtmlAttributeName("asp-action")]
		public string Action { get; set; }
		[HtmlAttributeName("asp-controller")]
		public string Controller { get; set; }
		[HtmlAttributeName("asp-area")]
		public string Area { get; set; }
		[HtmlAttributeName("fancybox")]
		public bool IsFancybox { get; set; }
		[HtmlAttributeName("delete-link")]
		public bool IsDeleteLink { get; set; }
		[HtmlAttributeName("asp-all-route-data", DictionaryAttributePrefix = "asp-route-")]
		public IDictionary<string, string> RouteValues
		{
			get
			{
				if(_routeValues == null) {
					_routeValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
				}

				return _routeValues;
			}
			set
			{
				_routeValues = value;
			}
		}
		[HtmlAttributeName("fb-all-values", DictionaryAttributePrefix ="fb-")]
		public IDictionary<string, string> FbValues
		{
			get
			{
				if(_fbValues == null) {
					_fbValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
				}

				return _fbValues;
			}
			set
			{
				_fbValues = value;
			}
		}
		public ActionLinkTagHelper(IHttpContextAccessor contextAccessor, IHtmlGenerator generator, IUrlHelperFactory urlHelperFactory) {
			_contextAccessor = contextAccessor;
			_generator = generator;
			_urlHelperFactory = urlHelperFactory;
		}

		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
			var urlHelper = _urlHelperFactory.GetUrlHelper(
				new ActionContext(_contextAccessor.HttpContext,
				new RouteData(),
				ViewContext.ActionDescriptor));

			var actionDescriptor = (ViewContext.ActionDescriptor as ControllerActionDescriptor);

			if(String.IsNullOrEmpty(Action)) 
				Action = actionDescriptor.ActionName;

			if(String.IsNullOrEmpty(Controller))
				Controller = actionDescriptor.ControllerName;

			if(!String.IsNullOrEmpty(Area))
				RouteValues.Add("area", Area);
			else
				RouteValues.Add("area", "");

			dynamic routeValues = new ExpandoObject();
			if(_routeValues != null && _routeValues.Count > 0) {
				foreach(var obj in _routeValues) {
					((System.Collections.Generic.IDictionary<string, object>)routeValues)[obj.Key] = obj.Value;
				}
			}

			dynamic fbValues = new ExpandoObject();
			if(_fbValues != null && _fbValues.Count > 0) {
				foreach(var obj in _fbValues) {
					((System.Collections.Generic.IDictionary<string, object>)fbValues)[$"fb-{obj.Key}"] = obj.Value;
				}
			}
			var color = String.Empty;

			if(!String.IsNullOrEmpty(Icon)) {
				if(Icon.Contains(":")) {
					var iconValues = Icon.Split(':');
					Icon = iconValues[0];
					color = iconValues[1];
				}
			}

			var cssClass = Icon ?? String.Empty;
			if(cssClass.StartsWith("fa-"))
				cssClass = $"fa {cssClass}";

			var icon = new FluentTagBuilder("i")
					.AddCssClass(cssClass);

			if(!String.IsNullOrEmpty(color))
				icon.MergeAttribute("style", $"color:{color}");

			var content = await output.GetChildContentAsync();
			var linkContent = content.GetContent();
			if(String.IsNullOrEmpty(linkContent))
				linkContent = caption;

			var link = _generator.GenerateActionLink(ViewContext, linkContent, Action, Controller, string.Empty, string.Empty, string.Empty, routeValues, null);
			var href = link.Attributes["href"];
			var anchor = new FluentTagBuilder("a")
					.MergeAttribute("href", href)
					.MergeAttributes(fbValues)
					.AppendHtml(icon)
					.Append(" ")
					.Append(linkContent);
			if(IsFancybox)
				anchor.AddCssClass("fancybox");
			if(IsDeleteLink)
				anchor.AddCssClass("remove-object");

			var builder = new FluentTagBuilder("li")
				.AppendHtml(anchor);

			output.TagName = "";
			
			output.Content.SetHtmlContent(builder);

		}
	}
}
