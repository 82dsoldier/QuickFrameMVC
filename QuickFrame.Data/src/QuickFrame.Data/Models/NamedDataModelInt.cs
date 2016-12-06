using QuickFrame.Data.Interfaces.Models;

namespace QuickFrame.Data.Models {

	/// <summary>
	/// The base class for data models with an Id of type Guid.
	/// </summary>
	public class NamedDataModelInt
		: DataModel<int>, INamedDataModelInt {
		/// <summary>
		/// The name field in the data model.  Should have a length of 256 characters.
		/// </summary>
		public string Name { get; set; }
	}
}