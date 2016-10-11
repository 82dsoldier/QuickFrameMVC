using System.Data.Entity.ModelConfiguration;

namespace QuickFrame.Security.AccountControl.Models.Configurations {

	public class SiteRoleConfiguration : EntityTypeConfiguration<SiteRole> {

		public SiteRoleConfiguration() {
			HasKey(x => x.Id);

			Property(x => x.Id).HasColumnName(@"Id").IsRequired().HasColumnType("nvarchar").HasMaxLength(128).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
			Property(x => x.Name).HasColumnName(@"Name").IsRequired().HasColumnType("nvarchar").HasMaxLength(256);
			Property(x => x.NormalizedName).HasColumnName(@"NormalizedName").IsRequired().HasColumnType("nvarchar").HasMaxLength(256);
			Property(x => x.ConcurrencyStamp).HasColumnName(@"ConcurrencyStamp").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
			Property(x => x.Description).HasColumnName(@"Description").IsOptional().HasColumnType("nvarchar").HasMaxLength(2048);
			HasMany(t => t.SiteRules).WithMany(t => t.SiteRoles).Map(m => {
				m.ToTable("RoleRules", "dbo");
				m.MapLeftKey("RoleId");
				m.MapRightKey("RuleId");
			});
		}
	}
}

// </auto-generated>