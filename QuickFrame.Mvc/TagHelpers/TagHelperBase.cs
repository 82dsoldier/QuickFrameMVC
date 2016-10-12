using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;

namespace QuickFrame.Mvc.TagHelpers {

	public class TagHelperBase : TagHelper {

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
		protected static readonly Dictionary<string, string> Rfc3339Formats =
			new Dictionary<string, string>(StringComparer.Ordinal)
			{
				{"date", "{0:yyyy-MM-dd}"},
				{"datetime", "{0:yyyy-MM-ddTHH:mm:ss.fffK}"},
				{"datetime-local", "{0:yyyy-MM-ddTHH:mm:ss.fff}"},
				{"time", "{0:HH:mm:ss.fff}"}
			}; protected IHtmlGenerator generator;

		[ViewContext]
		public ViewContext ViewContext { get; set; }

		public TagHelperBase(IHtmlGenerator htmlGenerator) {
			generator = htmlGenerator;
		}

		/// <summary>
		/// Generates the CheckBox.
		/// </summary>
		/// <param name="modelExplorer">The model explorer.</param>
		/// <param name="output">The output.</param>
		protected void GenerateCheckBox(ModelExplorer modelExplorer, TagHelperOutput output, ModelExpression forAttribute) {
			var htmlAttributes = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

			for(var i = 0; i < output.Attributes.Count; i++) {
				var attribute = output.Attributes[i];
				if(!htmlAttributes.ContainsKey(attribute.Name)) {
					htmlAttributes.Add(attribute.Name, attribute.Value);
				}
			}

			if(!htmlAttributes.ContainsKey("disabled"))
				htmlAttributes.Add("disabled", "disabled");

			var checkBoxTag = generator.GenerateCheckBox(
				ViewContext,
				modelExplorer,
				forAttribute.Name, null, htmlAttributes);
			if(checkBoxTag != null) {
				//output.Attributes.Clear();
				//output.TagName = null;

				//var renderingMode =
				//	output.TagMode == TagMode.SelfClosing ? TagRenderMode.SelfClosing : TagRenderMode.StartTag;
				checkBoxTag.TagRenderMode = TagRenderMode.SelfClosing;
				output.Content.SetHtmlContent(checkBoxTag);

				//var hiddenForCheckboxTag = generator.GenerateHiddenForCheckbox(ViewContext, modelExplorer, forAttribute.Name);
				//if(hiddenForCheckboxTag != null) {
				//	hiddenForCheckboxTag.TagRenderMode = TagRenderMode.SelfClosing;

				//	if(ViewContext.FormContext.CanRenderAtEndOfForm) {
				//		ViewContext.FormContext.EndOfFormContent.Add(hiddenForCheckboxTag);
				//	} else {
				//		output.Content.SetHtmlContent(hiddenForCheckboxTag);
				//	}
				//}
			}
		}

		/// <summary>
		/// Gets the type of the input.
		/// </summary>
		/// <param name="modelExplorer">The model explorer.</param>
		/// <param name="inputTypeHint">The input type hint.</param>
		/// <returns></returns>
		protected string GetInputType(ModelExplorer modelExplorer, out string inputTypeHint) {
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
		protected static IEnumerable<string> GetInputTypeHints(ModelExplorer modelExplorer) {
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
		protected string GetFormat(ModelExplorer modelExplorer, string inputTypeHint, string inputType) {
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

			if(inputType.Equals("email", StringComparison.CurrentCultureIgnoreCase)) {
				format = "<a href=\"mailto://{0}\">{0}</a>";
			}
			if(inputType.Equals("url", StringComparison.CurrentCultureIgnoreCase)) {
				format = "<a href=\"{0}\" target=\"_blank\">{0}</a>";
			}
			return format;
		}
	}
}