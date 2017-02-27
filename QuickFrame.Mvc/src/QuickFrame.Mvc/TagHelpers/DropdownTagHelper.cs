using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Mvc.TagHelpers
{
	[HtmlTargetElement("dropdown-menu")]
    public class DropdownTagHelper : TagHelper
    {
		[HtmlAttributeName("caption")]
		public string Caption { get; set; }
		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
			var content = await output.GetChildContentAsync();
			var outerList = new FluentTagBuilder("li")
				.AppendHtml(new FluentTagBuilder("a")
					.MergeAttribute("href", "#")
					.MergeAttribute("data-toggle", "dropdown")
					.MergeAttribute("role", "button")
					.AddCssClass("dropdown-toggle")
					.Append(Caption)
					.Append(" ")
					.AppendHtml(new FluentTagBuilder("span")
						.AddCssClass("caret")))
					.AppendHtml(new FluentTagBuilder("ul")
						.AddCssClass("dropdown-menu")
						.MergeAttribute("role", "menu")
						.AppendHtml(content));

			output.TagName = "";
			output.Content.Clear();
			output.Content.AppendHtml(outerList);
		}
	}
}
