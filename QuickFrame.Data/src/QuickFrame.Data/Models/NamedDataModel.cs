using QuickFrame.Data.Interfaces.Models;

namespace QuickFrame.Data.Models {

	public class NamedDataModel<TIdType>
		: DataModel<TIdType> {
	}
	/// <summary>
	/// An alias for NamedDataModel
	/// </summary>
	public class NamedDataModel
		: NamedDataModelInt, INamedDataModel {
	}
}