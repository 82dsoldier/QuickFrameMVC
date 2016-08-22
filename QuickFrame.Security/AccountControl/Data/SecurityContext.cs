using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using QuickFrame.Data;
using QuickFrame.Security.AccountControl.Data.Models;
using QuickFrame.Security.AccountControl.Data.Models.Configurations;
using System.Composition;
using System.Data.Entity;

namespace QuickFrame.Security.AccountControl.Data {

	/// <summary>
	/// The database context used to access security tables.
	/// </summary>
	/// <seealso cref="DcCommon.Data.TrackingContext"/>
	[Export(typeof(SecurityContext))]
	public class SecurityContext : TrackingContext {
		public DbSet<GroupRole> GroupRoles { get; set; }
		public DbSet<GroupRule> GroupRules { get; set; }
		public DbSet<SiteRole> SiteRoles { get; set; }
		public DbSet<SiteRoleClaim> SiteRoleClaims { get; set; }
		public DbSet<SiteRule> SiteRules { get; set; }
		public DbSet<SiteUserClaim> SiteUserClaims { get; set; }
		public DbSet<UserRole> UserRoles { get; set; }
		public DbSet<UserRule> UserRules { get; set; }
		public DbSet<RoleRule> RoleRules { get; set; }
		public SecurityContext(IOptions<DataOptions> configOptions, IHttpContextAccessor contextAccessor)
			: base(configOptions.Value.ConnectionString.Security, contextAccessor) {
		}

		public SecurityContext(string nameOrConnectionString)
			: base(nameOrConnectionString) {
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder) {
			base.OnModelCreating(modelBuilder);
			modelBuilder.Configurations.Add(new GroupRoleConfiguration());
			modelBuilder.Configurations.Add(new GroupRuleConfiguration());
			modelBuilder.Configurations.Add(new SiteRoleConfiguration());
			modelBuilder.Configurations.Add(new SiteRoleClaimConfiguration());
			modelBuilder.Configurations.Add(new SiteRuleConfiguration());
			modelBuilder.Configurations.Add(new SiteUserClaimConfiguration());
			modelBuilder.Configurations.Add(new UserRoleConfiguration());
			modelBuilder.Configurations.Add(new UserRuleConfiguration());
		}
	}
}