using System.Data.Entity.ModelConfiguration;

namespace QuickFrame.Security.AccountControl.Data.Models.Configurations {

	public class UserRoleConfiguration : EntityTypeConfiguration<UserRole> {

		public UserRoleConfiguration() {
			HasKey(x => new { x.UserId, x.RoleId });

			Property(x => x.UserId).HasColumnName(@"UserId").IsRequired().HasColumnType("nvarchar").HasMaxLength(128).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
			Property(x => x.RoleId).HasColumnName(@"RoleId").IsRequired().HasColumnType("nvarchar").HasMaxLength(128).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

			HasRequired(a => a.SiteRole).WithMany(b => b.UserRoles).HasForeignKey(c => c.RoleId).WillCascadeOnDelete(false);
		}
	}
}

// </auto-generated>