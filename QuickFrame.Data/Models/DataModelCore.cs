using QuickFrame.Data.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickFrame.Data.Models {

	public class DataModelCore<TDataType> : IDataModelCore<TDataType> {

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Required]
		public TDataType Id { get; set; }

		public bool IsDeleted { get; set; }
	}
}