using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using static QuickFrame.Mvc.FancyBoxSizeInfo;

namespace QuickFrame.Mvc.Tags {

	public class ActionLinkTagHelperBase : TagHelper {
		private IHtmlGenerator _generator;
		protected string htmlClass;

		[HtmlAttributeName("qf-controller")]
		public string Controller { get; set; }

		[HtmlAttributeName("qf-action")]
		public string Action { get; set; }

		[HtmlAttributeName("qf-all-route-data", DictionaryAttributePrefix = "qf-route-")]
		public IDictionary<string, string> RouteValues { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		[HtmlAttributeName("qf-size")]
		public FancyBoxSize WindowSize { get; set; } = FancyBoxSize.None;

		[HtmlAttributeNotBound]
		[ViewContext]
		public ViewContext ViewContext { get; set; }

		public override void Process(TagHelperContext context, TagHelperOutput output) {
			if(String.IsNullOrEmpty(Controller))
				Controller = ViewContext.RouteData.Values["controller"].ToString();

			if(String.IsNullOrEmpty(Action))
				Action = ViewContext.RouteData.Values["action"].ToString();

			var width = "";
			var height = "";

			if(WindowSize != FancyBoxSize.None) {
				width = FancyBoxSizes[WindowSize].Width;
				height = FancyBoxSizes[WindowSize].Height;
			}

			var routeValues = RouteValues.ToDictionary(
				kvp => kvp.Key,
				kvp => (object)kvp.Value,
				StringComparer.OrdinalIgnoreCase);

			TagBuilder link = _generator.GenerateActionLink(ViewContext,
				"",
				Action,
				Controller,
				String.Empty,
				String.Empty,
				String.Empty,
				routeValues,
				output.Attributes);

			if(!String.IsNullOrEmpty(width))
				link.MergeAttribute("data-width", width, true);

			if(!String.IsNullOrEmpty(height))
				link.MergeAttribute("data-height", height, true);

			link.AddCssClass(htmlClass);

			link.Attributes.Add("qf-fancybox", "");
			output.Content.AppendHtml(link);
		}

		public ActionLinkTagHelperBase(IHtmlGenerator generator) {
			_generator = generator;
		}
	}
}