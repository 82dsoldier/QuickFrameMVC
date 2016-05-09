﻿using QuickFrame.Data.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

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

	public class ConcurrentDataModelCore<TDataType> :
	DataModelCore<TDataType>, IConcurrentDataModelCore<TDataType> {

		[Timestamp]
		public byte[] RowVersion { get; set; }
	}

	public class ConcurrentDataModelInt
		: ConcurrentDataModelCore<int>,
		IConcurrentDataModelInt {
	}

	public class ConcurrentDataModelLong
		: ConcurrentDataModelCore<long>,
		IConcurrentDataModelLong {
	}

	public class ConcurrentDataModelGuid
		: ConcurrentDataModelCore<Guid>,
		IConcurrentDataModelGuid {
	}

	public class ConcurrentDataModel
		: ConcurrentDataModelInt,
		IConcurrentDataModel {
	}
}