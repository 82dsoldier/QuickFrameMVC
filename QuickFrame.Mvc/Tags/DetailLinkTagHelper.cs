using Microsoft.AspNet.Mvc.ViewFeatures;
using Microsoft.AspNet.Razor.TagHelpers;

namespace QuickFrame.Mvc.Tags {

	[HtmlTargetElement("qf-detail-link")]
	public class DetailLinkTagHelper : ActionLinkTagHelperBase {

		public DetailLinkTagHelper(IHtmlGenerator generator)
			: base(generator) {
			htmlClass = "fa fa-align-justify icon indent-15";
		}
	}
}