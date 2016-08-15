namespace QuickFrame.Data.Interfaces {

	public interface IConcurrentDataModel : IConcurrentDataModelInt {
	}

	public interface IConcurrentDataModel<TDataType>
: IDataModel<TDataType> {
		byte[] RowVersion { get; set; }
	}
}