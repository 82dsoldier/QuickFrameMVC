using System.Data.Entity.ModelConfiguration;

namespace QuickFrame.Security.AccountControl.Data.Models.Configurations {

	public class GroupRuleConfiguration : EntityTypeConfiguration<GroupRule> {

		public GroupRuleConfiguration() {
			HasKey(x => new { x.RuleId, x.GroupId });

			Property(x => x.RuleId).HasColumnName(@"RuleId").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
			Property(x => x.GroupId).HasColumnName(@"GroupId").IsRequired().HasColumnType("nvarchar").HasMaxLength(128).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

			HasRequired(a => a.SiteRule).WithMany(b => b.GroupRules).HasForeignKey(c => c.RuleId).WillCascadeOnDelete(false);
		}
	}
}

// </auto-generated>