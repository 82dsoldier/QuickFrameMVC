namespace QuickFrame.Mvc.Configuration {

	/// <summary>
	/// Stores options for service configuration.
	/// </summary>
	public class ServiceOptions {

		/// <summary>
		/// Gets or sets a value used to indicate loading services for sessions.
		/// </summary>
		public bool UseSession { get; set; }

		/// <summary>
		/// Gets or sets a value used to set the cookie name if sessions are enabled.
		/// </summary>
		public string CookieName { get; set; }
	}
}