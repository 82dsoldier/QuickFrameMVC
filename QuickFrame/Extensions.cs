using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;

namespace QuickFrame {

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
		public static string ToJsonString(this object obj) => JsonConvert.SerializeObject(obj);

		/// <summary>
		/// Strips the domain from a user name
		/// </summary>
		/// <param name="val">The username to strip.</param>
		/// <returns>The username without the domain.</returns>
		public static string StripDomain(this string val) {
			if(val == null)
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

		public static IEnumerable<KeyValuePair<string, string>> AsEnumerable(this IConfiguration configuration) {
			var stack = new Stack<IConfiguration>();
			stack.Push(configuration);
			while(stack.Count > 0) {
				var config = stack.Pop();
				var section = config as IConfigurationSection;
				if(section != null)
					yield return new KeyValuePair<string, string>(section.Path, section.Value);
				foreach(var child in config.GetChildren())
					stack.Push(child);
			}
		}

		public static string GetSid(this ClaimsPrincipal user)
			=> user.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.PrimarySid.ToString())?.Value;

		public static string ToHexString(this SecurityIdentifier sid) {
			byte[] buffer = new byte[sid.BinaryLength];
			sid.GetBinaryForm(buffer, 0);
			return buffer.ToHexString();
		}

		public static string ToHexString(this byte[] val) {
			StringBuilder sb = new StringBuilder(val.Length * 2);
			foreach(byte b in val)
				sb.AppendFormat("\\{0:X2}", b);
			return sb.ToString();
		}

		//public static bool ListIsNullOrEmpty<T>(this IList<T> list) {
		//	if(list == null)
		//		return true;
		//	if(list.Count == 0)
		//		return true;
		//	return false;
		//}

		//public static bool ListIsNullOrEmpty(this ResultPropertyValueCollection list) {
		//	if(list == null)
		//		return true;
		//	if(list.Count == 0)
		//		return true;
		//	return false;
		//}
	}
}