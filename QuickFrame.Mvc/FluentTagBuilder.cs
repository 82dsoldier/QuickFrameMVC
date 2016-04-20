using Microsoft.AspNet.Html.Abstractions;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewFeatures;
using Microsoft.Extensions.WebEncoders;
using System;
using System.Collections.Generic;
using System.IO;

namespace QuickFrame.Mvc {

	/// <summary>
	/// A new version of <see cref="Microsoft.AspNet.Mvc.Rendering.TagBuilder" /> in which commands used to set options on the tag builder can be chained together like
	/// Linq queries.
	/// </summary>
	/// <seealso cref="IHtmlContent" />
	public class FluentTagBuilder : IHtmlContent {

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

		/// <summary>
		/// The name of the tag being rendered.
		/// </summary>
		public string TagName => TagBuilder.TagName;

		//public static explicit operator FluentTagBuilder(TagBuilder v) => new FluentTagBuilder(v);
		public static implicit operator FluentTagBuilder(TagBuilder v) => new FluentTagBuilder(v);

		/// <summary>
		/// The <see cref="Microsoft.AspNet.Mvc.Rendering.TagRenderMode" /> used when this tag is rendered.
		/// </summary>
		public TagRenderMode TagRenderMode {
			get { return TagBuilder.TagRenderMode; }
			set { TagBuilder.TagRenderMode = value; }
		}

		/// <summary>
		/// Gets the inner HTML.
		/// </summary>
		/// <value>
		/// The inner HTML.
		/// </value>
		public IHtmlContentBuilder InnerHtml => TagBuilder.InnerHtml;

		/// <summary>
		/// Used to render the tag reprsened by this <see cref="FluentTagBuilder" /> object.
		/// </summary>
		/// <param name="writer">A <see cref="System.IO.TextWriter" /> object to which the tag will be rendered.</param>
		/// <param name="encoder">An <see cref="Microsoft.Extensions.WebEncoders.IHtmlEncoder" /> object used to encode the rendered object</param>
		public void WriteTo(TextWriter writer, IHtmlEncoder encoder) {
			TagBuilder.WriteTo(writer, encoder);
		}

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
		/// Sets the render mode.
		/// </summary>
		/// <param name="mode">The <see cref="Microsoft.AspNet.Mvc.Rendering.TagRenderMode" /></param>
		/// <returns>This <see cref="FluentTagBuilder" /> object</returns>
		public FluentTagBuilder SetRenderMode(TagRenderMode mode) {
			TagBuilder.TagRenderMode = mode;
			return this;
		}

		/// <summary>
		/// Adds an attribute to the rendering of the HTML tag.
		/// </summary>
		/// <param name="key">The name of the attribute.</param>
		/// <param name="value">The attribute value.</param>
		/// <returns>This <see cref="FluentTagBuilder" /> object</returns>
		public FluentTagBuilder AddAttribute(string key, string value) {
			TagBuilder.Attributes.Add(key, value);
			return this;
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

		/// <summary>
		/// Appends the specified HTML between the opening and closing tags.
		/// </summary>
		/// <param name="innerHtml">An <see cref="IHtmlContent" /> object</param>
		/// <returns>This <see cref="FluentTagBuilder" /> object</returns>
		public FluentTagBuilder Append(IHtmlContent innerHtml) {
			TagBuilder.InnerHtml?.Append(innerHtml);
			return this;
		}

		/// <summary>
		/// Appends the specified HTML between the opening and closing tags.
		/// </summary>
		/// <param name="innerHtml">A function that returns an <see cref="IHtmlContent" /> object</param>
		/// <returns>This <see cref="FluentTagBuilder" /> object</returns>
		public FluentTagBuilder Append(Func<IHtmlContent> innerHtml) {
			TagBuilder.InnerHtml.Append(innerHtml());
			return this;
		}

		/// <summary>
		/// Appends the specified HTML using the same method ast String.Format.
		/// </summary>
		/// <param name="formatString">The format string.</param>
		/// <param name="args">The arguments.</param>
		/// <returns>This <see cref="FluentTagBuilder" /> object</returns>
		public FluentTagBuilder AppendHtmlFormat(string formatString, params object[] args) {
			// ReSharper disable once MustUseReturnValue
			TagBuilder.InnerHtml.AppendFormat(formatString, args);
			return this;
		}

		/// <summary>
		/// Appends the HTML.
		/// </summary>
		/// <param name="encoded">The encoded.</param>
		/// <returns>This <see cref="FluentTagBuilder" /> object</returns>
		public FluentTagBuilder AppendHtml(string encoded) {
			TagBuilder.InnerHtml.AppendHtml(encoded);
			return this;
		}

		/// <summary>
		/// Appends the HTML line.
		/// </summary>
		/// <param name="encoded">The encoded.</param>
		/// <returns>This <see cref="FluentTagBuilder" /> object</returns>
		public FluentTagBuilder AppendHtmlLine(string encoded) {
			TagBuilder.InnerHtml.AppendHtmlLine(encoded);
			return this;
		}

		/// <summary>
		/// Appends the line.
		/// </summary>
		/// <returns>This <see cref="FluentTagBuilder" /> object</returns>
		public FluentTagBuilder AppendLine() {
			TagBuilder.InnerHtml.AppendLine();
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
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <param name="replaceExisting">if set to <c>true</c> replaces existing values.</param>
		/// <returns>This <see cref="FluentTagBuilder" /> object</returns>
		public FluentTagBuilder MergeAttributes(string key, string value, bool replaceExisting) {
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
	}
}