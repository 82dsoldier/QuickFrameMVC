using System;
using QuickFrame.Data.Interfaces.Dtos;
using QuickFrame.Data.Interfaces.Models;

namespace QuickFrame.Data.Dtos {

	public class ConcurrentDataTransferObjectInt<TSrc, TDest>
		: DataTransferObjectInt<TSrc, TDest>,
		IConcurrentDataTransferObjectInt
		where TSrc : class, IConcurrentDataModelInt {
		public string Name { get; set; }

		public byte[] RowVersion { get; set; }
	}
}