namespace QuickFrame.Data.Interfaces.Models {

	public interface IDataModel<TIdType> : IDataModelCore {
		TIdType Id { get; set; }
		bool IsDeleted { get; set; }
	}

	public interface IDataModel : IDataModelInt {
	}
}