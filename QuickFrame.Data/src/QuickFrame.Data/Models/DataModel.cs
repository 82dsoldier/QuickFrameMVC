using QuickFrame.Data.Interfaces.Models;

namespace QuickFrame.Data.Models {

	/// <summary>
	/// The base class for all data models.
	/// </summary>
	/// <typeparam name="TIdType">The type of Id that this class will implement.</typeparam>
	public class DataModel<TIdType> : IDataModel<TIdType>, IDataModelDeletable {
		/// <summary>
		/// The Id of the model.
		/// </summary>
		public TIdType Id { get; set; }
		/// <summary>
		/// True if the model has been deleted, false otherwise.
		/// </summary>
		public bool IsDeleted { get; set; }
	}

	/// <summary>
	/// An alias for the DataModelInt base class.
	/// </summary>
	public class DataModel : DataModel<int>, IDataModel {
	}
}