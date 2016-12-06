using QuickFrame.Data.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Data.Models
{
    public class ConcurrentDataModel<TIdType>
		: DataModel<TIdType>, IConcurrentDataModel<TIdType>
    {
		[Timestamp]
		public byte[] RowVersion { get; set; }
	}

	public class ConcurrentDataModel
		: ConcurrentDataModelInt, IConcurrentDataModel {

	}
}
