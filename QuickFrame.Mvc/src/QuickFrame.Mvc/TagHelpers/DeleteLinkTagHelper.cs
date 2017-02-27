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
	[HtmlTargetElement("delete-link", TagStructure = TagStructure.NormalOrSelfClosing)]
	public class DeleteLinkTagHelper : ActionLinkTagHelper {
		public DeleteLinkTagHelper(IHttpContextAccessor contextAccessor, IHtmlGenerator generator, IUrlHelperFactory urlHelperFactory) 
			: base(contextAccessor, generator, urlHelperFactory) {
			Icon = "fa-times:red";
			Action = "Delete";
			IsDeleteLink = true;
			caption = "Delete";
		}
	}
}
