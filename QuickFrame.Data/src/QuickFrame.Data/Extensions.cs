using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;

namespace QuickFrame.Data {
	
	///<summary>A static class containing extension methods used for manipulating data.</summary>
	public static class Extensions {
		private static Dictionary<int, int> _months = new Dictionary<int, int> {
			{1, 31 },
			{2, 28 },
			{3,31 },
			{4,30 },
			{5,31 },
			{6,30 },
			{7,31 },
			{8,31 },
			{9,30 },
			{10,31 },
			{11,30 },
			{12,31 }
		};
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

		public static DateTime AddMonths(this DateTime val, int months) {
			var year = val.Year;
			var month = val.Month + months;
			var day = val.Day;
			while(month > 12) {
				month -= 12;
				year += 1;
			}
			if(day > _months[month]) {
				day -= _months[month];
				month += 1;
				if(month > 12) {
					month -= 12;
					year += 1;
				}
			}
			return new DateTime(year, month, day, val.Hour, val.Minute, val.Second);
		}
	}
}