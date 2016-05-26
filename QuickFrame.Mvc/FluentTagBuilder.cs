using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;

namespace QuickFrame.Mvc {

	/// <summary>
	/// A new version of <see cref="Microsoft.AspNet.Mvc.Rendering.TagBuilder" /> in which commands used to set options on the tag builder can be chained together like
	/// Linq queries.
	/// </summary>
	/// <seealso cref="IHtmlContent" />
	public class FluentTagBuilder : IHtmlContent {

		/// <summary>
		/// Gets the tag builder that is used as a base for this class.
		/// </summary>
		/// <value>
		/// The tag builder.
		/// </value>
		private TagBuilder TagBuilder { get; }

		/// <summary>
		/// Gets an AttributeDictionary containing the attributes to be associated with the created tag.
		/// </summary>
		/// <value>
		/// An <see cref="Microsoft.AspNet.Mvc.ViewFeatures.AttributeDictionary" /> object
		/// </value>
		public AttributeDictionary Attributes => TagBuilder.Attributes;

		public IHtmlContentBuilder InnerHtml => TagBuilder.InnerHtml;

		public TagRenderMode TagRenderMode => TagBuilder.TagRenderMode;

		public string TagName => TagBuilder.TagName;

		/// <summary>
		/// Initializes a new instance of the <see cref="FluentTagBuilder"/> class.
		/// </summary>
		/// <param name="tagName">Name of the HTML tag to be created.</param>
		public FluentTagBuilder(string tagName)
			: this(new TagBuilder(tagName)) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FluentTagBuilder"/> class.
		/// </summary>
		/// <param name="tagBuilder">The tag builder on which to base this instance.</param>
		public FluentTagBuilder(TagBuilder tagBuilder) {
			TagBuilder = tagBuilder;
		}

		/// <summary>
		/// Adds the specified CSS classes to the rendered HTML tag.
		/// </summary>
		/// <param name="value">The CSS classes to add.</param>
		/// <returns>This <see cref="FluentTagBuilder" /> object</returns>
		public FluentTagBuilder AddCssClass(string value) {
			TagBuilder.AddCssClass(value);
			return this;
		}

		public static string CreateSanitizedId(string name, string invalidCharReplacement)
			=> TagBuilder.CreateSanitizedId(name, invalidCharReplacement);

		/// <summary>
		/// Generates the id for the rendered HTML tag.
		/// </summary>
		/// <param name="name">The name that will be used for the ID</param>
		/// <param name="invalidCharReplacement">string used to replace invanlid characters in the name</param>
		/// <returns>This <see cref="FluentTagBuilder" /> object</returns>
		public FluentTagBuilder GenerateId(string name, string invalidCharReplacement) {
			TagBuilder.GenerateId(name, invalidCharReplacement);
			return this;
		}

		/// <summary>
		/// Merges the specified attributes into the attribute dictionary.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <returns>This <see cref="FluentTagBuilder" /> object</returns>
		public FluentTagBuilder MergeAttribute(string key, string value) {
			TagBuilder.MergeAttribute(key, value);
			return this;
		}

		/// <summary>
		/// Merges the specified attributes into the attribute dictionary.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <param name="replaceExisting">A boolean indicating whether to replace any existing value</param>
		/// <returns>This <see cref="FluentTagBuilder" /> object</returns>
		public FluentTagBuilder MergeAttribute(string key, string value, bool replaceExisting) {
			TagBuilder.MergeAttribute(key, value, replaceExisting);
			return this;
		}

		/// <summary>
		/// Merges the specified attributes into the attribute dictionary.
		/// </summary>
		/// <typeparam name="TKey">The type of the key.</typeparam>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="attributes">The attributes.</param>
		/// <returns>This <see cref="FluentTagBuilder" /> object</returns>
		public FluentTagBuilder MergeAttributes<TKey, TValue>(IDictionary<TKey, TValue> attributes) {
			TagBuilder.MergeAttributes(attributes);
			return this;
		}

		/// <summary>
		/// Merges the specified attributes into the attribute dictionary.
		/// </summary>
		/// <typeparam name="TKey">The type of the key.</typeparam>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="attributes">The attributes.</param>
		/// <param name="replaceExisting">if set to <c>true</c> replaces existing values.</param>
		/// <returns>This <see cref="FluentTagBuilder" /> object</returns>
		public FluentTagBuilder MergeAttributes<TKey, TValue>(IDictionary<TKey, TValue> attributes, bool replaceExisting) {
			TagBuilder.MergeAttributes(attributes, replaceExisting);
			return this;
		}

		/// <summary>
		/// Merges the specified attributes into the attribute dictionary.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <param name="replaceExisting">if set to <c>true</c> replaces existing values.</param>
		/// <returns>This <see cref="FluentTagBuilder" /> object</returns>
		public FluentTagBuilder MergeAttributes(string key, string value, bool replaceExisting) {
			TagBuilder.MergeAttribute(key, value, replaceExisting);
			return this;
		}

		public FluentTagBuilder Append(string value) {
			TagBuilder.InnerHtml.Append(value);
			return this;
		}

		public FluentTagBuilder AppendFormat(string format, params object[] args) {
			TagBuilder.InnerHtml.AppendFormat(format, args);
			return this;
		}

		public FluentTagBuilder AppendHtml(IHtmlContent content) {
			TagBuilder.InnerHtml.AppendHtml(content);
			return this;
		}

		public FluentTagBuilder AppendHtmlLine(string value) {
			TagBuilder.InnerHtml.AppendHtmlLine(value);
			return this;
		}

		public void WriteTo(TextWriter writer, HtmlEncoder encoder) {
			TagBuilder.WriteTo(writer, encoder);
		}
	}
}