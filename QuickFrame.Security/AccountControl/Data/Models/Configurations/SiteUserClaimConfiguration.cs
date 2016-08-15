using System.Data.Entity.ModelConfiguration;

namespace QuickFrame.Security.AccountControl.Data.Models.Configurations {

	public class SiteUserClaimConfiguration : EntityTypeConfiguration<SiteUserClaim> {

		public SiteUserClaimConfiguration() {
			HasKey(x => x.Id);

			Property(x => x.Id).HasColumnName(@"Id").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
			Property(x => x.UserId).HasColumnName(@"UserId").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
			Property(x => x.ClaimType).HasColumnName(@"ClaimType").IsRequired().HasColumnType("nvarchar").HasMaxLength(2083);
			Property(x => x.ClaimValue).HasColumnName(@"ClaimValue").IsRequired().HasColumnType("nvarchar").HasMaxLength(256);
		}
	}
}

// </auto-generated>