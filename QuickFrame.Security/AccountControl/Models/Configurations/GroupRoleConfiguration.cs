using System.Data.Entity.ModelConfiguration;

namespace QuickFrame.Security.AccountControl.Models.Configurations {

	public class GroupRoleConfiguration : EntityTypeConfiguration<GroupRole> {

		public GroupRoleConfiguration() {
			HasKey(x => new { x.RoleId, x.GroupId });

			Property(x => x.RoleId).HasColumnName(@"RoleId").IsRequired().HasColumnType("nvarchar").HasMaxLength(128).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
			Property(x => x.GroupId).HasColumnName(@"GroupId").IsRequired().HasColumnType("nvarchar").HasMaxLength(128).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

			HasRequired(a => a.SiteRole).WithMany(b => b.GroupRoles).HasForeignKey(c => c.RoleId).WillCascadeOnDelete(false);
		}
	}
}

// </auto-generated>