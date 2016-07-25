using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace QuickFrame.Mvc.Tags {

	[HtmlTargetElement("qf-delete-link")]
	public class DeleteLinkTagHelper : ActionLinkTagHelperBase {

		public DeleteLinkTagHelper(IHtmlGenerator generator)
			: base(generator) {
			htmlClass = "fa fa-times icon indent-15 text-danger remove-object";
		}
	}
}