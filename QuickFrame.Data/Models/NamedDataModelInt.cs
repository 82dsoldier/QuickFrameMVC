using QuickFrame.Data.Interfaces.Models;

namespace QuickFrame.Data.Models {

	public class NamedDataModelInt
		: DataModel<int>, INamedDataModelInt {
		public string Name { get; set; }
	}
}