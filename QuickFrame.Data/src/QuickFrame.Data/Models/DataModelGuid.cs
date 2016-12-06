using QuickFrame.Data.Interfaces.Models;
using System;

namespace QuickFrame.Data.Models {

	/// <summary>
	/// The base class for data models with an Id of type Guid.
	/// </summary>
	public class DataModelGuid : DataModel<Guid>, IDataModelGuid {
	}
}