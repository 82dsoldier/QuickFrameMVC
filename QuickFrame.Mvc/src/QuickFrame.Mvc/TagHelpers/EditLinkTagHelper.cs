using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace QuickFrame.Mvc.TagHelpers
{
	[HtmlTargetElement("edit-link", TagStructure = TagStructure.NormalOrSelfClosing)]
	public class EditLinkTagHelper : ActionLinkTagHelper {
		public EditLinkTagHelper(IHttpContextAccessor contextAccessor, IHtmlGenerator generator, IUrlHelperFactory urlHelperFactory) 
			: base(contextAccessor, generator, urlHelperFactory) {
			Icon = "fa-align-justify";
			Action = "Edit";
			IsFancybox = true;
			caption = "Edit";
		}
	}
}
