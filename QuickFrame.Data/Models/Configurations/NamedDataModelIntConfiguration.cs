using QuickFrame.Data.Interfaces.Models;

namespace QuickFrame.Data.Models.Configurations {

	public class NamedDataModelIntConfiguration<TEntity>
		: DataModelIntConfiguration<TEntity>
		where TEntity : class, INamedDataModelInt {

		public NamedDataModelIntConfiguration()
			: base() {
			Property(x => x.Name).HasMaxLength(256);
		}
	}
}