using System.Data.Entity.ModelConfiguration;

namespace QuickFrame.Security.AccountControl.Data.Models.Configurations {

	public class UserRuleConfiguration : EntityTypeConfiguration<UserRule> {

		public UserRuleConfiguration() {
			HasKey(x => new { x.RuleId, x.UserId });

			Property(x => x.RuleId).HasColumnName(@"RuleId").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
			Property(x => x.UserId).HasColumnName(@"UserId").IsRequired().HasColumnType("nvarchar").HasMaxLength(128).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

			HasRequired(a => a.SiteRule).WithMany(b => b.UserRules).HasForeignKey(c => c.RuleId).WillCascadeOnDelete(false);
		}
	}
}

// </auto-generated>