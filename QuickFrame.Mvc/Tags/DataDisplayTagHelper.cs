using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
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
	public class DataDisplayTagHelper : TagHelper {

		/// <summary>
		/// The default input types
		/// </summary>
		private static readonly Dictionary<string, string> DefaultInputTypes =
			new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
			{
				{"Text", InputType.Text.ToString().ToLowerInvariant()},
				{"PhoneNumber", "tel"},
				{"Url", "url"},
				{"EmailAddress", "email"},
				{"Date", "date"},
				{"DateTime", "datetime"},
				{"DateTime-local", "datetime-local"},
				{"Time", "time"},
				{nameof(Byte), "number"},
				{nameof(SByte), "number"},
				{nameof(Int16), "number"},
				{nameof(UInt16), "number"},
				{nameof(Int32), "number"},
				{nameof(UInt32), "number"},
				{nameof(Int64), "number"},
				{nameof(UInt64), "number"},
				{nameof(Single), InputType.Text.ToString().ToLowerInvariant()},
				{nameof(Double), InputType.Text.ToString().ToLowerInvariant()},
				{nameof(Boolean), InputType.CheckBox.ToString().ToLowerInvariant()},
				{nameof(Decimal), InputType.Text.ToString().ToLowerInvariant()},
				{nameof(String), InputType.Text.ToString().ToLowerInvariant()}
			};

		/// <summary>
		/// The default RFC3339 formats for the specified data types
		/// </summary>
		private static readonly Dictionary<string, string> Rfc3339Formats =
			new Dictionary<string, string>(StringComparer.Ordinal)
			{
				{"date", "{0:yyyy-MM-dd}"},
				{"datetime", "{0:yyyy-MM-ddTHH:mm:ss.fffK}"},
				{"datetime-local", "{0:yyyy-MM-ddTHH:mm:ss.fff}"},
				{"time", "{0:HH:mm:ss.fff}"}
			};

		/// <summary>
		/// Initializes a new instance of the <see cref="DisplaySpanTagHelper"/> class.
		/// </summary>
		/// <param name="generator">The generator.</param>
		public DataDisplayTagHelper(IHtmlGenerator generator) {
			Generator = generator;
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
		/// Gets or sets the view context.
		/// </summary>
		/// <value>
		/// The view context.
		/// </value>
		[HtmlAttributeNotBound]
		[ViewContext]
		public ViewContext ViewContext { get; set; }

		/// <summary>
		/// Gets the generator.
		/// </summary>
		/// <value>
		/// The generator.
		/// </value>
		protected IHtmlGenerator Generator { get; }

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
				GenerateCheckBox(modelExplorer, output);
			} else {
				var val = string.IsNullOrEmpty(Value) ? ForAttribute.Model : Value;
				var formattedVal = string.IsNullOrEmpty(format)
					? Convert.ToString(val, CultureInfo.CurrentCulture)
					: string.Format(format, val);
				output.Content.Append(formattedVal);
			}
		}

		/// <summary>
		/// Generates the CheckBox.
		/// </summary>
		/// <param name="modelExplorer">The model explorer.</param>
		/// <param name="output">The output.</param>
		private void GenerateCheckBox(ModelExplorer modelExplorer, TagHelperOutput output) {
			var htmlAttributes = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

			for(var i = 0; i < output.Attributes.Count; i++) {
				var attribute = output.Attributes[i];
				if(!htmlAttributes.ContainsKey(attribute.Name)) {
					htmlAttributes.Add(attribute.Name, attribute.Value);
				}
			}

			if(!htmlAttributes.ContainsKey("disabled"))
				htmlAttributes.Add("disabled", "disabled");

			var checkBoxTag = Generator.GenerateCheckBox(
				ViewContext,
				modelExplorer,
				ForAttribute.Name, null, htmlAttributes);
			if(checkBoxTag != null) {
				output.Attributes.Clear();
				output.TagName = null;

				var renderingMode =
					output.TagMode == TagMode.SelfClosing ? TagRenderMode.SelfClosing : TagRenderMode.StartTag;
				checkBoxTag.TagRenderMode = renderingMode;
				output.Content.AppendHtml(checkBoxTag);

				var hiddenForCheckboxTag = Generator.GenerateHiddenForCheckbox(ViewContext, modelExplorer, ForAttribute.Name);
				if(hiddenForCheckboxTag != null) {
					hiddenForCheckboxTag.TagRenderMode = renderingMode;

					if(ViewContext.FormContext.CanRenderAtEndOfForm) {
						ViewContext.FormContext.EndOfFormContent.Add(hiddenForCheckboxTag);
					} else {
						output.Content.AppendHtml(hiddenForCheckboxTag);
					}
				}
			}
		}

		/// <summary>
		/// Gets the type of the input.
		/// </summary>
		/// <param name="modelExplorer">The model explorer.</param>
		/// <param name="inputTypeHint">The input type hint.</param>
		/// <returns></returns>
		private string GetInputType(ModelExplorer modelExplorer, out string inputTypeHint) {
			foreach(var hint in GetInputTypeHints(modelExplorer)) {
				string inputType;
				if(DefaultInputTypes.TryGetValue(hint, out inputType)) {
					inputTypeHint = hint;
					return inputType;
				}
			}

			inputTypeHint = InputType.Text.ToString().ToLowerInvariant();
			return inputTypeHint;
		}

		/// <summary>
		/// Gets the input type hints.
		/// </summary>
		/// <param name="modelExplorer">The model explorer.</param>
		/// <returns></returns>
		private static IEnumerable<string> GetInputTypeHints(ModelExplorer modelExplorer) {
			if(!string.IsNullOrEmpty(modelExplorer.Metadata.TemplateHint)) {
				yield return modelExplorer.Metadata.TemplateHint;
			}

			if(!string.IsNullOrEmpty(modelExplorer.Metadata.DataTypeName)) {
				yield return modelExplorer.Metadata.DataTypeName;
			}

			var fieldType = modelExplorer.ModelType;
			if(typeof(bool?) != fieldType) {
				fieldType = modelExplorer.Metadata.UnderlyingOrModelType;
			}

			foreach(var typeName in TemplateRenderer.GetTypeNames(modelExplorer.Metadata, fieldType)) {
				yield return typeName;
			}
		}

		/// <summary>
		/// Gets the format.
		/// </summary>
		/// <param name="modelExplorer">The model explorer.</param>
		/// <param name="inputTypeHint">The input type hint.</param>
		/// <param name="inputType">Type of the input.</param>
		/// <returns></returns>
		private string GetFormat(ModelExplorer modelExplorer, string inputTypeHint, string inputType) {
			string format;
			string rfc3339Format;
			if(string.Equals("decimal", inputTypeHint, StringComparison.OrdinalIgnoreCase) &&
				string.Equals("text", inputType, StringComparison.Ordinal) &&
				string.IsNullOrEmpty(modelExplorer.Metadata.EditFormatString)) {
				format = "{0:0.00}";
			} else if(Rfc3339Formats.TryGetValue(inputType, out rfc3339Format) &&
					   ViewContext.Html5DateRenderingMode == Html5DateRenderingMode.Rfc3339 &&
					   !modelExplorer.Metadata.HasNonDefaultEditFormat &&
					   (typeof(DateTime) == modelExplorer.ModelType || typeof(DateTimeOffset) == modelExplorer.ModelType)) {
				format = rfc3339Format;
			} else {
				format = modelExplorer.Metadata.DisplayFormatString;
			}

			return format;
		}
	}
}