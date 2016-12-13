using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using QuickFrame.Data;
using QuickFrame.Security.AccountControl.Models;
using QuickFrame.Security.Data;
#if NETSTANDARD1_6
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
#else
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
#endif

namespace QuickFrame.Security.AccountControl.Data {

	public class SecurityContext : TrackingContext {
		//public DbSet<GroupRole> GroupRoles { get; set; }
		//public DbSet<GroupRule> GroupRules { get; set; }
		public DbSet<SiteRole> SiteRoles { get; set; }
		public DbSet<SiteRoleClaim> SiteRoleClaims { get; set; }
		public DbSet<SiteRule> SiteRules { get; set; }
		public DbSet<UserRole> UserRoles { get; set; }
		public DbSet<UserRule> UserRules { get; set; }
		//public DbSet<RoleRule> RoleRules { get; set; }
		//public DbSet<SiteGroup> SiteGroups { get; set; }

#if NETSTANDARD1_6
		public SecurityContext(DbContextOptions options) : base(options) {
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.Entity<GroupRole>(obj => {
				obj.HasKey(a => new { a.RoleId, a.GroupId });
				obj.Property(a => a.RoleId).IsRequired().HasMaxLength(128).ValueGeneratedNever();
				obj.Property(a => a.GroupId).IsRequired().HasMaxLength(128).ValueGeneratedNever();
				obj.HasOne(a => a.SiteRole).WithMany(b => b.GroupRoles).HasForeignKey(c => c.RoleId).OnDelete(DeleteBehavior.SetNull);
			});

			modelBuilder.Entity<GroupRule>(obj => {
				obj.HasKey(a => new { a.RuleId, a.GroupId });
				obj.Property(a => a.RuleId).IsRequired().ValueGeneratedNever();
				obj.Property(a => a.GroupId).IsRequired().ValueGeneratedNever();
				obj.HasOne(a => a.SiteRule).WithMany(b => b.GroupRules).OnDelete(DeleteBehavior.SetNull);
			});

			modelBuilder.Entity<SiteRoleClaim>(obj => {
				obj.HasKey(a => a.Id);
				obj.Property(a => a.Id).IsRequired().UseSqlServerIdentityColumn();
				obj.Property(a => a.RoleId).HasMaxLength(128);
				obj.Property(a => a.ClaimType).IsRequired().HasMaxLength(2083);
				obj.Property(a => a.ClaimValue).IsRequired().HasMaxLength(256);
				obj.HasOne(a => a.SiteRole).WithMany(b => b.SiteRoleClaims).HasForeignKey(c => c.RoleId).OnDelete(DeleteBehavior.SetNull);
			});

			modelBuilder.Entity<SiteRole>(obj => {
				obj.HasKey(a => a.Id);
				obj.Property(a => a.Id).IsRequired().HasMaxLength(128).UseSqlServerIdentityColumn();
				obj.Property(a => a.Name).IsRequired().HasMaxLength(256);
				obj.Property(a => a.NormalizedName).IsRequired().HasMaxLength(256);
				obj.Property(a => a.ConcurrencyStamp).HasMaxLength(128);
				obj.Property(a => a.Description).HasMaxLength(2048);
			});

			modelBuilder.Entity<RoleRule>(obj => {
				obj.HasKey(a => new { a.RuleId, a.RoleId });
				obj.Property(a => a.RuleId).IsRequired().ValueGeneratedNever();
				obj.Property(a => a.RoleId).IsRequired().HasMaxLength(128).ValueGeneratedNever();
				obj.HasOne(a => a.SiteRule).WithMany(b => b.RoleRules).HasForeignKey(c => c.RuleId).OnDelete(DeleteBehavior.SetNull);
				obj.HasOne(a => a.SiteRole).WithMany(b => b.RoleRules).HasForeignKey(c => c.RoleId).OnDelete(DeleteBehavior.SetNull);
			});

			modelBuilder.Entity<SiteRule>(obj => {
				obj.HasKey(a => a.Id);
				obj.Property(a => a.Id).IsRequired().UseSqlServerIdentityColumn();
				obj.Property(a => a.Url).IsRequired().HasMaxLength(128);
				obj.Property(a => a.Priority).IsRequired();
				obj.Property(a => a.IsAllow).IsRequired();
				obj.Property(a => a.IsDeleted).IsRequired();
			});

			modelBuilder.Entity<UserRole>(obj => {
				obj.HasKey(a => new { a.UserId, a.RoleId });
				obj.Property(a => a.UserId).IsRequired().HasMaxLength(128).ValueGeneratedNever();
				obj.Property(a => a.RoleId).IsRequired().HasMaxLength(128).ValueGeneratedNever();
				obj.HasOne(a => a.SiteRole).WithMany(b => b.UserRoles).HasForeignKey(c => c.RoleId).OnDelete(DeleteBehavior.SetNull);
			});

			modelBuilder.Entity<UserRule>(obj => {
				obj.HasKey(a => new { a.RuleId, a.UserId });
				obj.Property(a => a.UserId).IsRequired().HasMaxLength(128).ValueGeneratedNever();
				obj.Property(a => a.RuleId).IsRequired().ValueGeneratedNever();
				obj.HasOne(a => a.SiteRule).WithMany(b => b.UserRules).HasForeignKey(c => c.RuleId).OnDelete(DeleteBehavior.SetNull);
			});
		}
#else
		public SecurityContext(IOptions<DataOptions> configOptions, IHttpContextAccessor contextAccessor)
			: base(configOptions.Value.ConnectionString.Security, contextAccessor) {
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder) {
			//modelBuilder.Entity<GroupRole>().HasKey(a => new { a.RoleId, a.GroupId });
			//modelBuilder.Entity<GroupRole>().Property(a => a.RoleId).IsRequired().HasMaxLength(128).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			//modelBuilder.Entity<GroupRole>().Property(a => a.GroupId).IsRequired().HasMaxLength(128).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			//modelBuilder.Entity<GroupRole>().HasRequired(a => a.SiteRole).WithMany(b => b.GroupRoles).HasForeignKey(c => c.RoleId).WillCascadeOnDelete(false);

			//modelBuilder.Entity<GroupRule>().HasKey(a => new { a.RuleId, a.GroupId });
			//modelBuilder.Entity<GroupRule>().Property(a => a.RuleId).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			//modelBuilder.Entity<GroupRule>().Property(a => a.GroupId).IsRequired().HasMaxLength(128).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			//modelBuilder.Entity<GroupRule>().HasRequired(a => a.SiteRule).WithMany(b => b.GroupRules).HasForeignKey(c => c.RuleId).WillCascadeOnDelete(false);

			modelBuilder.Entity<SiteRoleClaim>().HasKey(a => a.Id);
			modelBuilder.Entity<SiteRoleClaim>().Property(a => a.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			modelBuilder.Entity<SiteRoleClaim>().Property(a => a.RoleId).IsOptional().HasMaxLength(128);
			modelBuilder.Entity<SiteRoleClaim>().Property(a => a.ClaimType).IsRequired().HasMaxLength(2083);
			modelBuilder.Entity<SiteRoleClaim>().Property(a => a.ClaimValue).IsRequired().HasMaxLength(256);
			modelBuilder.Entity<SiteRoleClaim>().HasOptional(a => a.SiteRole).WithMany(b => b.SiteRoleClaims).HasForeignKey(c => c.RoleId).WillCascadeOnDelete(false);

			modelBuilder.Entity<SiteRole>().HasKey(a => a.Id);
			modelBuilder.Entity<SiteRole>().Property(a => a.Id).IsRequired().HasMaxLength(128);
			modelBuilder.Entity<SiteRole>().Property(a => a.Name).IsRequired().HasMaxLength(256);
			modelBuilder.Entity<SiteRole>().Property(a => a.NormalizedName).IsRequired().HasMaxLength(256);
			modelBuilder.Entity<SiteRole>().Property(a => a.ConcurrencyStamp).IsOptional().HasMaxLength(128);
			modelBuilder.Entity<SiteRole>().Property(a => a.Description).IsOptional().HasMaxLength(2048);
			modelBuilder.Entity<SiteRole>().HasMany(a => a.SiteRules)
			.WithMany(b => b.SiteRoles)
			.Map(c => {
				 c.ToTable("RoleRules");
				 c.MapLeftKey("RoleId");
				 c.MapRightKey("RuleId");
			 });

			//modelBuilder.Entity<RoleRule>().ToTable("RoleRules");
			//modelBuilder.Entity<RoleRule>().HasKey(a => new { a.RoleId, a.RuleId });
			//modelBuilder.Entity<RoleRule>().Property(a => a.RoleId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			//modelBuilder.Entity<RoleRule>().Property(a => a.RuleId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			//modelBuilder.Entity<RoleRule>().HasRequired(a => a.SiteRole)
			//	.WithMany(b => b.RoleRules)
			//	.HasForeignKey(a => a.RoleId);
			//modelBuilder.Entity<RoleRule>().HasRequired(a => a.SiteRule)
			//	.WithMany(b => b.RoleRules)
			//	.HasForeignKey(a => a.RuleId);

			modelBuilder.Entity<SiteRule>().HasKey(a => a.Id);
			modelBuilder.Entity<SiteRule>().Property(a => a.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			modelBuilder.Entity<SiteRule>().Property(a => a.Url).IsRequired().HasMaxLength(128);
			modelBuilder.Entity<SiteRule>().Property(a => a.Priority).IsRequired();
			modelBuilder.Entity<SiteRule>().Property(a => a.IsAllow).IsRequired();
			modelBuilder.Entity<SiteRule>().Property(a => a.IsDeleted).IsRequired();

			modelBuilder.Entity<UserRole>().HasKey(a => new { a.UserId, a.RoleId });
			modelBuilder.Entity<UserRole>().Property(a => a.UserId).IsRequired().HasMaxLength(128).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			modelBuilder.Entity<UserRole>().Property(a => a.RoleId).IsRequired().HasMaxLength(128).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			modelBuilder.Entity<UserRole>().HasRequired(a => a.SiteRole).WithMany(b => b.UserRoles).HasForeignKey(c => c.RoleId).WillCascadeOnDelete(false);

			modelBuilder.Entity<UserRule>().HasKey(a => new { a.RuleId, a.UserId });
			modelBuilder.Entity<UserRule>().Property(a => a.UserId).IsRequired().HasMaxLength(128).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			modelBuilder.Entity<UserRule>().Property(a => a.RuleId).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			modelBuilder.Entity<UserRule>().HasRequired(a => a.SiteRule).WithMany(b => b.UserRules).HasForeignKey(c => c.RuleId).WillCascadeOnDelete(false);

			base.OnModelCreating(modelBuilder);
		}
#endif

		//protected override void OnModelCreating(DbModelBuilder modelBuilder) {
		//	base.OnModelCreating(modelBuilder);
		//	modelBuilder.Configurations.Add(new GroupRoleConfiguration());
		//	modelBuilder.Configurations.Add(new GroupRuleConfiguration());
		//	modelBuilder.Configurations.Add(new SiteRoleConfiguration());
		//	modelBuilder.Configurations.Add(new SiteRoleClaimConfiguration());
		//	modelBuilder.Configurations.Add(new SiteRuleConfiguration());
		//	modelBuilder.Configurations.Add(new UserRoleConfiguration());
		//	modelBuilder.Configurations.Add(new UserRuleConfiguration());
		//}
	}
}