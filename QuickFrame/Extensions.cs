using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace QuickFrame
{
	/// <summary>
	/// A static class containing general extensions
	/// </summary>
	public static class Extensions {
		private static readonly Regex DomainMatch = new Regex("^(?:.*\\\\)?([^@]*)(?:@.*)?");
		/// <summary>
		/// Converts an existing string to a JSON formatted string
		/// </summary>
		/// <param name="obj">The object to be converted</param>
		/// <returns>A JSON formatted string</returns>
		public static string ToJsonString(this object obj) => new JavaScriptSerializer().Serialize(obj);

		/// <summary>
		/// Strips the domain from a user name
		/// </summary>
		/// <param name="val">The username to strip.</param>
		/// <returns>The username without the domain.</returns>
		public static string StripDomain(this string val) {
			if (val == null)
				return string.Empty;
			var m = DomainMatch.Match(val);
			return m.Groups[1].Value;
		}
		/// <summary>
		/// Determines whether the specified string is numeric.
		/// </summary>
		/// <param name="val">The string to test.</param>
		/// <returns>True or False to indicate if the string is numeric.</returns>
		public static bool IsNumeric(this string val) {
			int number;
			return int.TryParse(val, out number);
		}
	}
}
