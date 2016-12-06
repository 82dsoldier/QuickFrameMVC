using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace QuickFrame.Security.Configuration {

	public class NameListOptions {
		private Dictionary<string, bool> _excludedNames = new Dictionary<string, bool>();

		public void Load(IConfigurationSection config) {
			foreach(var configSection in config.AsEnumerable()) {
				if(configSection.Key.StartsWith("ExcludedNames:"))
					_excludedNames.Add(configSection.Key.Substring(configSection.Key.IndexOf(":") + 1), Convert.ToBoolean(configSection.Value));
			}
		}

		public bool IsValid(string name) {
			var exclude = false;
			foreach(var exclusion in _excludedNames) {
				if(exclusion.Value)
					exclude = true;
				Regex re = new Regex(exclusion.Key);
				var isMatch = re.IsMatch(name);
				if(exclusion.Value && isMatch)
					return true;

				if(!exclusion.Value && isMatch)
					return false;
			}

			return !exclude;
		}
	}
}