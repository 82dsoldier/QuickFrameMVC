using System.Data.Entity.ModelConfiguration;

namespace QuickFrame.Data.Models.Configurations {

	public class ConfigurationInt<TModel> :
		EntityTypeConfiguration<TModel>
		where TModel : DataModelInt {

		public ConfigurationInt() {
			HasKey(x => x.Id);
			Property(x => x.Id).HasColumnName(@"Id").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
			Property(x => x.IsDeleted).HasColumnName(@"IsDeleted").IsRequired().HasColumnType("bit");
		}
	}
}