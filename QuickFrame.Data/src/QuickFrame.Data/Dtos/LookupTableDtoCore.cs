using QuickFrame.Data.Interfaces.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Data.Dtos
{
    public class LookupTableDtoCore<TIdType> : IDataTransferObjectCore {
		public TIdType Id { get; set; }
		public string Name { get; set; }

		public void Register() {
			throw new NotImplementedException();
		}
	}
}
