using Microsoft.AspNetCore.Http;
using System;

namespace QuickFrame.Mvc {

	public static class Extensions {

		public static bool? GetBoolean(this ISession session, string key, bool? defaultValue = null) {
			var data = session.Get(key);
			if(data == null)
				return defaultValue;
			return BitConverter.ToBoolean(data, 0);
		}

		public static void SetBoolean(this ISession session, string key, bool value) {
			session.Set(key, BitConverter.GetBytes(value));
		}
	}
}
