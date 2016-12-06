using Newtonsoft.Json;
using System.Security.Principal;
using System.Text;

namespace QuickFrame.Data {

	///<summary>A static class containing extension methods used for manipulating data.</summary>
	public static class Extensions {

		///<summary>Serializes the specified object to a JSON format string.</summary>
		public static string ToJsonString(this object obj) => JsonConvert.SerializeObject(obj, Formatting.Indented);

		public static T FromJsonString<T>(this string obj) => JsonConvert.DeserializeObject<T>(obj);

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

		public static bool IsNumeric(this string val) {
			int number;
			return int.TryParse(val, out number);
		}
	}
}