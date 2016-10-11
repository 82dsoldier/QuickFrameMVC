using QuickFrame.Data.Interfaces.Models;

namespace QuickFrame.Data.Models {

	public class NamedDataModel<TIdType>
		: DataModel<TIdType> {
	}

	public class NamedDataModel
		: NamedDataModelInt, INamedDataModel {
	}
}