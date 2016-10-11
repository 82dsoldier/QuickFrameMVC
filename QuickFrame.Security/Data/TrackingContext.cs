using EntityFramework.MappingAPI.Extensions;
using ExpressMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using QuickFrame.Data;
using QuickFrame.Security.Data.Events;
using QuickFrame.Security.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Claims;

namespace QuickFrame.Security.Data {

	///<summary>
	///	A <see cref="System.Data.Entity.DbContext">DbContext</see> that allows all database transactions to be logged.
	///</summary>
	///<remarks>
	///	Other database contexts should inherit from TrackingContext to provide per-transaction logging.  This will provide all the features of a standard <see cref="System.Data.Entity.DbContext">DbContext</see> with the additional feature of logging each change made through the context.  Please note that this does not log changes made to the databsae through other methods such as SQL Manager.  A proper security policy for your database will be needed to maintain database integrity.
	///</remarks>
	public class TrackingContext : DbContext {
		protected readonly IHttpContextAccessor _contextAccessor;
		private DataChangedEventHandler _dataChanged;

		public event DataChangedEventHandler DataChanged
		{
			add
			{
				_dataChanged += value;
			}
			remove
			{
				_dataChanged -= value;
			}
		}

		///<summary>Used to create a new instance of the TrackingContext class.</summary>
		///<param name="nameOrConnectionString">The connection string used to access the database.</param>
		///<param name="contextAccessor">
		///	The <see cref="Microsoft.AspNetCore.Http.IHttpContextAccessor">IHttpContextAccessor</see> used to gather information about the user making the databsae changes.
		///</param>
		public TrackingContext(string nameOrConnectionString, IHttpContextAccessor contextAccessor)
			: base(nameOrConnectionString) {
			_contextAccessor = contextAccessor;
		}

		///<summary>Used to create a new instance of the TrackingContext class.</summary>
		///<param name="configOptions">
		///	An <see cref="Microsoft.Extensions.Options.IOptions{TOptions}">IOptions</see>&lt;<see cref="QuickFrame.Data.DataOptions">DataOptions</see>&gt; object used to import the data options and select a connection string from them.
		///</param>
		///<param name="contextAccessor">
		///	The <see cref="Microsoft.AspNetCore.Http.IHttpContextAccessor">IHttpContextAccessor</see> used to gather information about the user making the databsae changes.
		///</param>
		public TrackingContext(IOptions<DataOptions> configOptions, IHttpContextAccessor contextAccessor)
			: base(configOptions.Value.ConnectionString.Default) {
			_contextAccessor = contextAccessor;
		}

		///<summary>Used to create a new instance of the TrackingContext class.</summary>
		///<param name="nameOrConnectionString">The connection string used to access the database.</param>
		public TrackingContext(string nameOrConnectionString)
			: base(nameOrConnectionString) {
		}

		///<summary>Gets or sets a boolean value indicating if change tracking is on.</summary>
		public bool TrackChanges { get; set; } = false;
		public bool Notify { get; set; } = false;
		///<summary>
		///	A <see cref="System.Data.Entity.DbSet{TEntity}">DbSet&lt;TEntity&gt;</see> of <see cref="QuickFrame.Data.Models.AuditLog">AuditLogs</see> representing the logging table.
		///</summary>
		public DbSet<AuditLog> AuditLogs { get; set; }

		///<summary>Overrides the default SaveChanges method so that every time changes are saved, they are logged.</summary>
		///<returns>The number of state entries written to the underlying database.</returns>
		public override int SaveChanges() {
			if(!TrackChanges)
				return base.SaveChanges();
			string userId = (_contextAccessor.HttpContext.User.Identity as ClaimsIdentity).Claims.First(claim => claim.Type == ClaimTypes.PrimarySid).Value;
			var changeList = ChangeTracker.Entries()
						.Where(p => p.State == EntityState.Added || p.State == EntityState.Deleted || p.State == EntityState.Modified)
						.SelectMany(entity => GetAuditRecordsForChange(entity, userId)).ToList();
			foreach(var log in changeList)
				AuditLogs.Add(log);
//			Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
			var ret = base.SaveChanges();

			foreach(var log in changeList)
				_dataChanged?.Invoke(this, Mapper.Map<AuditLog, DataChangedEventArgs>(log));

			return ret;
		}

		private IEnumerable<AuditLog> GetAuditRecordsForChange(DbEntityEntry entity, string userId) {
			var changeTime = DateTime.UtcNow;

			var entityType = ObjectContext.GetObjectType(entity.Entity.GetType());

			var tableAttr = entityType.GetCustomAttributes(typeof(TableAttribute), true).SingleOrDefault() as TableAttribute;
			var tableName = tableAttr?.Name ?? entityType.Name;

			var keys = this.Db(entityType).Pks.Select(x => x.PropertyName);
			var keyName = String.Empty;
			if(keys != null) {
				if(keys.Count() > 1)
					keyName = String.Join(":", keys);
				else
					keyName = keys.First();
			}

			var recordId = string.Empty;
			if(entity.State != EntityState.Added) {
				if(keyName.Contains(":")) {
					var keyNames = keyName.Split(':');
					var keyList = new List<string>();
					foreach(var name in keyNames)
						keyList.Add(entity.OriginalValues.GetValue<object>(name).ToString());
					recordId = string.Join(":", keyList);
				} else {
					recordId = entity.OriginalValues.GetValue<object>(keyName).ToString();
				}
			} else {
				recordId = null;
			}

			if(entity.State == EntityState.Modified) {
				foreach(var propertyName in entity.CurrentValues.PropertyNames.Where(propertyName => !Equals(entity.OriginalValues.GetValue<object>(propertyName), entity.CurrentValues.GetValue<object>(propertyName)))) {
					var log = new AuditLog {
						UserId = userId,
						EventDate = changeTime,
						EventType = (int)entity.State,
						TableName = tableName,
						RecordId = recordId,
						ColumnName = propertyName,
						NewValue = entity.CurrentValues.GetValue<object>(propertyName)?.ToJsonString(),
						OriginalValue =
							entity.State == EntityState.Added ? null : entity.OriginalValues.GetValue<object>(propertyName)?.ToJsonString()
					};
					yield return log;
				}
			} else {
				var log = new AuditLog {
					UserId = userId,
					EventDate = changeTime,
					EventType = (int)entity.State,
					TableName = tableName,
					RecordId = recordId,
					ColumnName = "*ALL",
					NewValue =
						entity.State != EntityState.Added
							? entity.OriginalValues.ToObject()?.ToJsonString()
							: entity.CurrentValues.ToObject()?.ToJsonString()
				};
				yield return log;
			}
		}
	}
}