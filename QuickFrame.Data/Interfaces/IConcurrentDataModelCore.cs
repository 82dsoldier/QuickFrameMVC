namespace QuickFrame.Data.Interfaces {

	public interface IConcurrentDataModelCore<TDataType>
	: IDataModelCore<TDataType> {
		byte[] RowVersion { get; set; }
	}
}