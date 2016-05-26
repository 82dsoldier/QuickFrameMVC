using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace QuickFrame.Mvc.Tags {

	[HtmlTargetElement("form", Attributes = "qf-form")]
	public class FormBuilderTagHelper : TagHelper {

		[HtmlAttributeName("qf-antiforgery")]
		public bool? Antiforgery { get; set; }

		[HtmlAttributeNotBound]
		[ViewContext]
		public ViewContext ViewContext { get; set; }

		protected IHtmlGenerator Generator;

		public FormBuilderTagHelper(IHtmlGenerator generator) {
			Generator = generator;
		}

		public override void Process(TagHelperContext context, TagHelperOutput output) {
			TagBuilder tagBuilder = Generator.GenerateForm(ViewContext, ViewContext.RouteData.Values["action"].ToString(),
				ViewContext.RouteData.Values["controller"].ToString(), null, null, null);
			output.MergeAttributes(tagBuilder);

			if(Antiforgery == true)
				output.PostContent.AppendHtml(Generator.GenerateAntiforgery(ViewContext));
		}
	}
}