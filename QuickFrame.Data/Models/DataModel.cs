using QuickFrame.Data.Interfaces.Models;

namespace QuickFrame.Data.Models {

	public class DataModel<TIdType> : IDataModel<TIdType>, IDataModelDeletable {
		public TIdType Id { get; set; }
		public bool IsDeleted { get; set; }
	}

	public class DataModel : DataModel<int>, IDataModel {
	}
}