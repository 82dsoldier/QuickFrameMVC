using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace QuickFrame.Data.Models.Configurations {

	public class ConfigurationGuid<TModel> :
		EntityTypeConfiguration<TModel>
		where TModel : DataModelGuid {

		public ConfigurationGuid() {
			HasKey(x => x.Id);
			Property(x => x.Id).HasColumnName(@"Id").IsRequired().HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			Property(x => x.IsDeleted).HasColumnName(@"IsDeleted").IsRequired().HasColumnType("bit");
		}
	}
}