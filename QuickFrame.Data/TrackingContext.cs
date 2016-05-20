using Microsoft.AspNet.Http;
using Microsoft.Extensions.OptionsModel;
using QuickFrame.Configuration;
using QuickFrame.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace QuickFrame.Data {

	/// <summary>
	/// A DbContext that allows logging of all database transactions run through entity framework.
	/// </summary>
	/// <seealso cref="System.Data.Entity.DbContext"/>
	public class TrackingContext : DbContext {
		private readonly IHttpContextAccessor _contextAccessor;

		/// <summary>
		/// Initializes a new instance of the <see cref="TrackingContext"/> class.
		/// </summary>
		/// <param name="nameOrConnectionString">Either the database name or a connection string.</param>
		/// <param name="contextAccessor">
		/// The <see cref="IHttpContextAccessor"/> allowing access to the current HttpContext and the
		/// current user information.
		/// </param>
		public TrackingContext(string nameOrConnectionString, IHttpContextAccessor contextAccessor)
			: base(nameOrConnectionString) {
			_contextAccessor = contextAccessor;
		}

		public TrackingContext(IOptions<DataOptions> configOptions, IHttpContextAccessor contextAccessor)
			: base(configOptions.Value.ConnectionString.Default) {
			_contextAccessor = contextAccessor;
		}

		public TrackingContext(string nameOrConnectionString)
			: base(nameOrConnectionString) {
		}

		/// <summary>
		/// Gets or sets a value indicating whether to track all database transactions.
		/// </summary>
		/// <value><c>true</c> if database changes are to be tracked; otherwise, <c>false</c>.</value>
		public bool TrackChanges { get; set; } = false;

		public DbSet<AuditLog> AuditLogs { get; set; }

		/// <summary>
		/// Saves all changes made in this context to the underlying database.
		/// </summary>
		/// <returns>
		/// The number of state entries written to the underlying database. This can include state
		/// entries for entities and/or relationships. Relationship state entries are created for
		/// many-to-many relationships and relationships where there is no foreign key property
		/// included in the entity class (often referred to as independent associations).
		/// </returns>
		public override int SaveChanges() {
			if (!TrackChanges) return base.SaveChanges();
			foreach (
				var log in
					ChangeTracker.Entries()
						.Where(p => p.State == EntityState.Added || p.State == EntityState.Deleted || p.State == EntityState.Modified)
						.SelectMany(entity => GetAuditRecordsForChange(entity, _contextAccessor?.HttpContext.User.Identity.Name)))
				AuditLogs.Add(log);
			return base.SaveChanges();
		}

		/// <summary>
		/// Generates the audit log records that will be added to the database.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <param name="userId">The user identifier.</param>
		/// <returns></returns>
		private IEnumerable<AuditLog> GetAuditRecordsForChange(DbEntityEntry entity, string userId) {
			var changeTime = DateTime.UtcNow;

			var tableAttr =
				entity.Entity.GetType().GetCustomAttributes(typeof(TableAttribute), true).SingleOrDefault() as TableAttribute;
			var tableName = tableAttr?.Name ?? entity.Entity.GetType().Name;

			var objectContext = ((IObjectContextAdapter)this).ObjectContext;
			var keyName = ((IEnumerable<EdmMember>)objectContext.MetadataWorkspace
				.GetType(entity.Entity.GetType().Name, entity.Entity.GetType().Namespace, DataSpace.CSpace)
				.MetadataProperties
				.First(mp => mp.Name == "KeyMembers")
				.Value)?.ToList()[0]?.Name;

			if (entity.State == EntityState.Modified) {
				foreach (var propertyName in entity.CurrentValues.PropertyNames.Where(propertyName => !Equals(entity.OriginalValues.GetValue<object>(propertyName), entity.CurrentValues.GetValue<object>(propertyName)))) {
					yield return new AuditLog {
						UserId = userId,
						EventDate = changeTime,
						EventType = (int)entity.State,
						TableName = tableName,
						RecordId = entity.CurrentValues.GetValue<object>(keyName).ToString(),
						ColumnName = propertyName,
						NewValue = entity.CurrentValues.GetValue<object>(propertyName)?.ToJsonString(),
						OriginalValue =
							entity.State == EntityState.Added ? null : entity.OriginalValues.GetValue<object>(propertyName)?.ToJsonString()
					};
				}
			} else {
				yield return new AuditLog {
					UserId = userId,
					EventDate = changeTime,
					EventType = (int)entity.State,
					TableName = tableName,
					RecordId = entity.State != EntityState.Added ? entity.OriginalValues.GetValue<object>(keyName).ToString() : null,
					ColumnName = "*ALL",
					NewValue =
						entity.State != EntityState.Added
							? entity.OriginalValues.ToObject()?.ToJsonString()
							: entity.CurrentValues.ToObject()?.ToJsonString()
				};
			}
		}
	}
}