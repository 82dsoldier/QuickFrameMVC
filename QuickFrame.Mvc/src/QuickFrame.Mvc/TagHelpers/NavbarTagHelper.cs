using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Mvc.TagHelpers
{
	[HtmlTargetElement("navbar")]
    public class NavbarTagHelper : TagHelper
    {
		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
			var content = await output.GetChildContentAsync();
			FluentTagBuilder builder = new FluentTagBuilder("div")
				.AppendHtml(new FluentTagBuilder("ul")
					.AddCssClass("nav navbar-nav")
					.AppendHtml(content));
			output.TagName = "";
			output.Content.SetHtmlContent(builder);
		}
	}
}
