using QuickFrame.Data.Interfaces.Dtos;
using System;

namespace QuickFrame.Data.Dtos {
	[Obsolete("Use LookupTableDtoCore<TIdType> or one of the non-generic types instead.")]
	public class LookupTableDto<TSrc, TIdType> : IDataTransferObjectCore {
		public TIdType Id { get; set; }
		public string Name { get; set; }

		public void Register() {
			throw new NotImplementedException();
		}
	}

	public class LookupTableDto : LookupTableDtoInt {

	}
}