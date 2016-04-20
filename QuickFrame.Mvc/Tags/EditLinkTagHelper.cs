using Microsoft.AspNet.Mvc.ViewFeatures;
using Microsoft.AspNet.Razor.TagHelpers;

namespace QuickFrame.Mvc.Tags {

	[HtmlTargetElement("qf-edit-link")]
	public class EditLinkTagHelper : ActionLinkTagHelperBase {

		public EditLinkTagHelper(IHtmlGenerator generator)
			: base(generator) {
			htmlClass = "fa fa-pencil-square-o indent-15";
		}
	}
}