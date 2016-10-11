namespace QuickFrame.Data.Interfaces.Dtos {

	public interface INamedDataTransferObjectInt : IDataTransferObjectInt {
		string Name { get; set; }
	}
}