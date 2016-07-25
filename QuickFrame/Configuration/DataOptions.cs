using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuickFrame.Configuration {

	/// <summary>
	/// The options from the appsettings.json file that provide database information to the controllers
	/// </summary>
	public class DataOptions {

		/// <summary>
		/// Gets or sets the connection string.
		/// </summary>
		/// <value>
		/// The connection string.
		/// </value>
		public ConnectionStringData ConnectionString { get; set; } = new ConnectionStringData();

		public bool UseFilestream { get; set; } = false;

		public void Load(IConfigurationRoot config) {
			foreach(var configSection in config.AsEnumerable()) {
				if(configSection.Key.StartsWith("Data:", StringComparison.CurrentCultureIgnoreCase) && !String.IsNullOrEmpty(configSection.Value)) {
					if(configSection.Key.Contains("UseFilestream")) {
						bool val = false;
						Boolean.TryParse(configSection.Value, out val);
						UseFilestream = val;
					} else {
						var keys = configSection.Key.Split(':');
						ConnectionString[keys[1]] = configSection.Value;
					}
				}
			}
		}
	}

	public class ConnectionStringData {
		private Dictionary<string, string> _connectionStringList;

		public string Default
		{
			get
			{
				if(_connectionStringList == null)
					return null;
				if(_connectionStringList.Count == 0)
					return null;
				if(_connectionStringList.ContainsKey("ConnectionString"))
					return _connectionStringList["ConnectionString"];
				if(_connectionStringList.ContainsKey("DefaultConnectionString"))
					return _connectionStringList["DefaultConnectionString"];
				return _connectionStringList.First().Value;
			}
		}

		public string Security
		{
			get
			{
				if(_connectionStringList == null)
					return null;
				if(_connectionStringList.Count == 0)
					return null;
				if(_connectionStringList.ContainsKey("Security"))
					return _connectionStringList["Security"];
				return Default;
			}
		}

		public string Attachments
		{
			get
			{
				if(_connectionStringList == null)
					return null;
				if(_connectionStringList.Count == 0)
					return null;
				if(_connectionStringList.ContainsKey("Attachments"))
					return _connectionStringList["Attachments"];
				return Default;
			}
		}
		public string this[string index]
		{
			get
			{
				if(_connectionStringList == null)
					return null;
				return _connectionStringList.ContainsKey(index) ? _connectionStringList[index] : null;
			}
			set
			{
				if(_connectionStringList == null)
					_connectionStringList = new Dictionary<string, string>();
				_connectionStringList.Add(index, value);
			}
		}
	}
}