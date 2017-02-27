using QuickFrame.Data.Interfaces.Dtos;
using QuickFrame.Data.Interfaces.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickFrame.Data.Dtos {

	public class DataTransferObject<TSrc, TDest, TIdType>
		: DataTransferObjectCore<TSrc, TDest>,
		IDataTransferObject<TIdType>
		where TSrc : class, IDataModel<TIdType> {
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public TIdType Id { get; set; }
		public bool IsDeleted { get; set; }
	}

	public class DataTransferObject<TSrc, TDest>
		: DataTransferObjectInt<TSrc, TDest>,
		IDataTransferObject
		where TSrc : class, IDataModel {
	}
}