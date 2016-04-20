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
		public string ConnectionString { get; set; }
	}
}