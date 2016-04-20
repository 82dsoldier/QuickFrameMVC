using QuickFrame.Data.Interfaces;
using System;

namespace QuickFrame.Data {

	public class DataModelCore<TDataType>
		: IDataModelCore<TDataType> {
		public TDataType Id { get; set; }

		public bool IsDeleted { get; set; }
	}

	public class DataModelInt
		: IDataModelInt {
		public int Id { get; set; }
		public bool IsDeleted { get; set; }
	}

	public class DataModelLong
	: IDataModelLong {
		public long Id { get; set; }
		public bool IsDeleted { get; set; }
	}

	public class DataModelGuid
	: IDataModelGuid {
		public Guid Id { get; set; }
		public bool IsDeleted { get; set; }
	}

	public class DataModel
	: IDataModelInt {
		public int Id { get; set; }
		public bool IsDeleted { get; set; }
	}
}