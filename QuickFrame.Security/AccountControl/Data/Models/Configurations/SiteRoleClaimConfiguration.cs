using System.Data.Entity.ModelConfiguration;

namespace QuickFrame.Security.AccountControl.Data.Models.Configurations {

	public class SiteRoleClaimConfiguration : EntityTypeConfiguration<SiteRoleClaim> {

		public SiteRoleClaimConfiguration() {
			HasKey(x => x.Id);

			Property(x => x.Id).HasColumnName(@"Id").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
			Property(x => x.RoleId).HasColumnName(@"RoleId").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
			Property(x => x.ClaimType).HasColumnName(@"ClaimType").IsRequired().HasColumnType("nvarchar").HasMaxLength(2083);
			Property(x => x.ClaimValue).HasColumnName(@"ClaimValue").IsRequired().HasColumnType("nvarchar").HasMaxLength(256);

			HasOptional(a => a.SiteRole).WithMany(b => b.SiteRoleClaims).HasForeignKey(c => c.RoleId).WillCascadeOnDelete(false);
		}
	}
}

// </auto-generated>