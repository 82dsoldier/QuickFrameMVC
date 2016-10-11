using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuickFrame.Data {

	/// <summary>
	/// Class that accepts database configuration options from the appsettings.json file.
	/// </summary>
	/// <remarks>
	/// This class contains information needed for database connections including connection strings.
	/// </remarks>
	public class DataOptions {

		///<summary>A <see cref="QuickFrame.Data.ConnectionStringData"></see> class used to store and retrieve relavant database connection strings in the appsettings.json file.</summary>
		public ConnectionStringData ConnectionString { get; set; } = new ConnectionStringData();

		///<summary>A boolean value indicating if a FileStream database will be used.</summary>
		public bool UseFilestream { get; set; } = false;

		///<summary>A function used to load the database data into the DataOptions class for later retrieval.</summary>
		///<remarks>
		///	The Load function takes an <see cref="Microsoft.Extensions.Configuration.IConfigurationRoot">IConfigurationRoot</see> and enumerates it for any database settings.  If a section key begins with Data: it is considered to be a database setting.  The name of the key (without Data: prepended) will be used to store and identify the connection string in the DataOptions class.
		///</remarks>
		///<param name="config">
		///	An <see cref="Microsoft.Extensions.Configuration.IConfigurationRoot">IConfigurationRoot</see> representing the values within the appsettings.json.
		///</param>
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

	///<summary>Class used to store and enumerate database connection strings</summary>
	public class ConnectionStringData {
		private Dictionary<string, string> _connectionStringList;

		///<summary>Gets the default connection string.</summary>
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
				if(_connectionStringList.ContainsKey("DefaultConnection"))
					return _connectionStringList["DefaultConnection"];
				return _connectionStringList.First().Value;
			}
		}

		///<summary>Gets the security database connection string or the default if no security database has been set.</summary>
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

		///<summary>Gets the active directory connection string.</summary>
		public string AdSecurity
		{
			get
			{
				if(_connectionStringList == null || _connectionStringList.Count == 0)
					return null;
				if(_connectionStringList.ContainsKey("AdSecurity"))
					return _connectionStringList["AdSecurity"];
				if(_connectionStringList.ContainsKey("Security"))
					return _connectionStringList["Security"];
				return Default;
			}
		}

		///<summary>Gets the attachments database connection string or the default if no attachments database has been set.</summary>
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

		///<summary>Provides an indexer through which connection strings can be accessed by name.  If a specified connection string does not exist, the default connection string is returned instead.</summary>
		public string this[string index]
		{
			get
			{
				if(_connectionStringList == null)
					return null;
				return _connectionStringList.ContainsKey(index) ? _connectionStringList[index] : _connectionStringList["DefaultConnection"];
				;
			}
			set
			{
				if(_connectionStringList == null)
					_connectionStringList = new Dictionary<string, string>();
				if(!_connectionStringList.ContainsKey(index))
					_connectionStringList.Add(index, value);
				else
					_connectionStringList[index] = value;
			}
		}
	}
}