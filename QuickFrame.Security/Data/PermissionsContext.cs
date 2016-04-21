﻿using Microsoft.AspNet.Http;
using Microsoft.Extensions.OptionsModel;
using QuickFrame.Configuration;
using QuickFrame.Data;
using QuickFrame.Security.Data.Models;
using System.ComponentModel.Composition;
using System.Data.Entity;

namespace QuickFrame.Security.Data {

	/// <summary>
	/// The database context used to access security tables.
	/// </summary>
	/// <seealso cref="DcCommon.Data.TrackingContext"/>
	[Export(typeof(PermissionsContext))]
	public class PermissionsContext : TrackingContext {

		/// <summary>
		/// Initializes a new instance of the <see cref="PermissionsContext"/> class.
		/// </summary>
		/// <param name="configOptions">The configuration options.</param>
		/// <param name="contextAccessor"></param>
		/// <remarks>
		/// If the object is created through the DI container, the configOptions parameter will be
		/// resolved to a DataConfigurationOptionns class that can be used to retrieve the connection
		/// string for the base class.
		/// </remarks>
		public PermissionsContext(IOptions<DataOptions> configOptions, IHttpContextAccessor contextAccessor)
			: base(configOptions.Value.ConnectionString, contextAccessor) {
		}

		/// <summary>
		/// Gets or sets the list of users with specific permissions to the site.
		/// </summary>
		/// <value>The site users.</value>
		public DbSet<SiteUser> SiteUsers { get; set; }

		/// <summary>
		/// Gets or sets the list of roles that can be assigned to users of the site.
		/// </summary>
		/// <value>The site roles.</value>
		public DbSet<SiteRole> SiteRoles { get; set; }

		public DbSet<SiteGroup> SiteGroups { get; set; }

		/// <inheritdoc/>
		protected override void OnModelCreating(DbModelBuilder modelBuilder) {
			modelBuilder.Entity<SiteUser>()
				.HasMany(t => t.Roles)
				.WithMany(t => t.Users)
				.Map(
					t => {
						t.ToTable("UserRoles");
						t.MapLeftKey("UserId");
						t.MapRightKey("RoleId");
					});

			modelBuilder.Entity<SiteGroup>()
				.HasMany(t => t.Roles)
				.WithMany(t => t.Groups)
				.Map(
					t => {
						t.ToTable("GroupRoles");
						t.MapLeftKey("GroupId");
						t.MapRightKey("RoleId");
					});
		}
	}
}