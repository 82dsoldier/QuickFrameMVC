using System.Data.Entity.ModelConfiguration;

namespace QuickFrame.Security.AccountControl.Models.Configurations {

	public class SiteRuleConfiguration : EntityTypeConfiguration<SiteRule> {

		public SiteRuleConfiguration() {
			HasKey(x => x.Id);

			Property(x => x.Id).HasColumnName(@"Id").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
			Property(x => x.Url).HasColumnName(@"Url").IsRequired().HasColumnType("nvarchar").HasMaxLength(2083);
			Property(x => x.Priority).HasColumnName(@"Priority").IsRequired().HasColumnType("int");
			Property(x => x.IsAllow).HasColumnName(@"IsAllow").IsRequired().HasColumnType("bit");
			Property(x => x.IsDeleted).HasColumnName(@"IsDeleted").IsRequired().HasColumnType("bit");
		}
	}
}

// </auto-generated>