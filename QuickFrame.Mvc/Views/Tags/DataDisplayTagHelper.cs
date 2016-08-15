using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Globalization;

namespace QuickFrame.Mvc.Tags {

	/// <summary>
	/// Takes the name specified in the display-for attribute and displays the property associated with the name.
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />
	[HtmlTargetElement("span", Attributes = "display-for")]
	[HtmlTargetElement("div", Attributes = "display-for")]
	[HtmlTargetElement("p", Attributes = "display-for")]
	[HtmlTargetElement("td", Attributes = "display-for")]
	public class DataDisplayTagHelper : TagHelperBase {

		/// <summary>
		/// Initializes a new instance of the <see cref="DisplaySpanTagHelper"/> class.
		/// </summary>
		/// <param name="generator">The generator.</param>
		public DataDisplayTagHelper(IHtmlGenerator generator)
			: base(generator) {
		}

		/// <summary>
		/// Gets or sets the name of the property for which to search
		/// </summary>
		/// <value>
		/// For attribute.
		/// </value>
		[HtmlAttributeName("display-for")]
		public ModelExpression ForAttribute { get; set; }

		/// <summary>
		/// Gets or sets the format used to display the data.
		/// </summary>
		/// <value>
		/// The format.
		/// </value>
		[HtmlAttributeName("display-format")]
		public string Format { get; set; }

		/// <summary>
		/// Gets or sets the name of the input type.
		/// </summary>
		/// <value>
		/// The name of the input type.
		/// </value>
		[HtmlAttributeName("type")]
		public string InputTypeName { get; set; }

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>
		/// The value.
		/// </value>
		[HtmlAttributeName("value")]
		public string Value { get; set; }

		/// <summary>
		/// Synchronously executes the <see cref="T:Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" /> with the given <paramref name="context" /> and
		/// <paramref name="output" />.
		/// </summary>
		/// <param name="context">Contains information associated with the current HTML tag.</param>
		/// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
		/// <exception cref="System.ArgumentNullException">
		/// </exception>
		/// <exception cref="System.InvalidOperationException"></exception>
		public override void Process(TagHelperContext context, TagHelperOutput output) {
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			if(output == null)
				throw new ArgumentNullException(nameof(output));

			if(ForAttribute == null)
				return;

			if(InputTypeName != null)
				output.CopyHtmlAttribute("type", context);

			if(Value != null)
				output.CopyHtmlAttribute(nameof(Value), context);

			var metadata = ForAttribute.Metadata;
			var modelExplorer = ForAttribute.ModelExplorer;

			if(metadata == null) {
				throw new InvalidOperationException(
					string.Format("The {2} was unable to provide metadata about '{1}' expression value '{3}' for {0}.",
						"<input>",
						"qf-for",
						nameof(IModelMetadataProvider),
						ForAttribute.Name));
			}

			string inputType;
			string inputTypeHint;
			if(string.IsNullOrEmpty(InputTypeName)) {
				inputType = GetInputType(modelExplorer, out inputTypeHint);
			} else {
				inputType = InputTypeName.ToLowerInvariant();
				inputTypeHint = null;
			}

			var format = string.IsNullOrEmpty(Format) ? GetFormat(modelExplorer, inputTypeHint, inputType) : Format;

			if(format == "checkbox") {
				GenerateCheckBox(modelExplorer, output, ForAttribute);
			} else {
				var val = string.IsNullOrEmpty(Value) ? ForAttribute.Model : Value;
				var formattedVal = string.IsNullOrEmpty(format)
					? Convert.ToString(val, CultureInfo.CurrentCulture)
					: string.Format(format, val);
				output.PostContent.AppendHtml(formattedVal);
			}
		}
	}
}