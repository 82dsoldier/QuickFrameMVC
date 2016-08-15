using QuickFrame.Data.Interfaces;

namespace QuickFrame.Data.Dtos {

	public class ReadOnlyDataTransferObject<TSrc, TDest> : GenericDataTransferObject<TSrc, TDest>, IReadOnlyDataTransferObject<TSrc, TDest> {
	}
}