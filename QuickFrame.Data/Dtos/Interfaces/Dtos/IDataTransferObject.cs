namespace QuickFrame.Data.Interfaces.Dtos {

	public interface IDataTransferObject<TIdType>
		: IDataTransferObjectCore {
		TIdType Id { get; set; }
		bool IsDeleted { get; set; }
	}

	public interface IDataTransferObject : IDataTransferObjectInt {
	}
}