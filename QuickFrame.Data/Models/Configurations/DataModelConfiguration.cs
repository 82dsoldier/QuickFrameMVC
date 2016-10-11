using QuickFrame.Data.Interfaces.Models;

namespace QuickFrame.Data.Models.Configurations {

	public class DataModelConfiguration<TEntity>
		: DataModelIntConfiguration<TEntity>
		where TEntity : class, IDataModel {
	}
}