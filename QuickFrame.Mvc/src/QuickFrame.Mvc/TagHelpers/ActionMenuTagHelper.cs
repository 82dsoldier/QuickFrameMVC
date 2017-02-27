using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Mvc.TagHelpers
{
	[HtmlTargetElement("action-menu", TagStructure=TagStructure.NormalOrSelfClosing)]
    public class ActionMenuTagHelper : TagHelper
    {
		[ViewContext]
		public ViewContext ViewContext { get; set; }
		[HtmlAttributeName("asp-icon")]
		public string Icon { get; set; }

		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
			var cssClass = String.IsNullOrEmpty(Icon) ? "fa-ellipsis-v" : Icon;
			if(cssClass.StartsWith("fa-"))
				cssClass = $"fa {cssClass}";

			var content = await output.GetChildContentAsync();

			FluentTagBuilder builder = new FluentTagBuilder("div")
				.AddCssClass("dropdown")
				.AppendHtml(new FluentTagBuilder("a")
					.MergeAttribute("style", "display:inline-block;width:100%")
					.MergeAttribute("data-toggle", "dropdown")
					.MergeAttribute("href", "#")
					.AppendHtml(new FluentTagBuilder("i")
						.AddCssClass(cssClass)))
				.AppendHtml(new FluentTagBuilder("ul")
					.AddCssClass("dropdown-menu")
					.AppendHtml(content));
			output.TagName = "";
			output.Content.SetHtmlContent(builder);
		}
	}
}
