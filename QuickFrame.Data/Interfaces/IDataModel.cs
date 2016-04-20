using System;

namespace QuickFrame.Data.Interfaces {

	/// <summary>
	/// Interface to be implemented by any data entity providing an Id field and an IsDeleted boolean flag.
	/// </summary>
	/// <remarks>
	/// When creating entities for database tables, each entity should have an Id field that is the primary key and a unique identifier
	/// as well as a boolean field indicating if the item has been deleted.  Implementing this interface allows the use of the entities
	/// in the generic data manipulation classses provided by the IDataService interface.
	/// </remarks>
	public interface IDataModelCore<TDataType> {

		/// <summary>
		/// Gets or sets the unique identifier for the model.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		TDataType Id { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is deleted.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
		/// </value>
		bool IsDeleted { get; set; }
	}

	public interface IDataModelInt : IDataModelCore<int> {
	}

	public interface IDataModelLong : IDataModelCore<long> {
	}

	public interface IDataModelGuid : IDataModelCore<Guid> {
	}

	public interface IDataModel : IDataModelInt {
	}
}