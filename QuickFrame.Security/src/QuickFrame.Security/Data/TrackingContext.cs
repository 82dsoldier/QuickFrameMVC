using ExpressMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using QuickFrame.Data;
using QuickFrame.Security.Data.Events;
using QuickFrame.Security.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
#if NETSTANDARD1_6
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
#else
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using EntityFramework.MappingAPI.Extensions;
using System.Data.Entity.Core.Objects;
#endif

namespace QuickFrame.Security.Data {

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

#if NETSTANDARD1_6
		public TrackingContext(DbContextOptions options) : base(options) {

		}
#else
		public TrackingContext(IOptions<DataOptions> configOptions, IHttpContextAccessor contextAccessor)
			: base(configOptions.Value.ConnectionString.Default) {
			_contextAccessor = contextAccessor;
		}

		public TrackingContext(string nameOrCOnnectionString, IHttpContextAccessor contextAccessor) : base(nameOrCOnnectionString) {
			_contextAccessor = contextAccessor;
		}
#endif

		public bool TrackChanges { get; set; } = false;
		public bool Notify { get; set; } = false;
		public DbSet<AuditLog> AuditLogs { get; set; }

#if NETSTANDARD1_6
		public override int SaveChanges() {
			if(!TrackChanges) 
				return base.SaveChanges();
			string userId = (_contextAccessor.HttpContext.User.Identity as ClaimsIdentity).Claims.First(claim => claim.Type == ClaimTypes.PrimarySid).Value;
			var changeList = ChangeTracker.Entries()
						.Where(p => p.State == EntityState.Added || p.State == EntityState.Deleted || p.State == EntityState.Modified)
						.SelectMany(entity => GetAuditRecordsForChange(entity, userId)).ToList();
			foreach(var log in changeList)
				AuditLogs.Add(log);
			var ret = base.SaveChanges();
			foreach(var log in changeList)
				_dataChanged?.Invoke(this, Mapper.Map<AuditLog, DataChangedEventArgs>(log));
			return ret;
		}

		private IEnumerable<AuditLog> GetAuditRecordsForChange(EntityEntry entity, string userId) {
			var changeTime = DateTime.UtcNow;
			var entityType = entity.Metadata.ClrType;
			var dbEntityType = Model.GetEntityTypes().First(t => t.Name == entityType.Name); 
			var tableName = dbEntityType.SqlServer().TableName;
			var keys = dbEntityType.FindPrimaryKey().Properties;
			var keyName = String.Empty;
			if(keys != null) {
				if(keys.Count() > 1)
					keyName = String.Join(":", keys.Select(k => k.Name));
				else
					keyName = keys.First().Name;
			}

			var recordId = string.Empty;
			if(entity.State != EntityState.Added) {
				if(keyName.Contains(":")) {
					var keyNames = keyName.Split(':');
					var keyList = new List<string>();
					foreach(var name in keyNames)
						keyList.Add(entity.Property(name).OriginalValue.ToString());
					recordId = string.Join(":", keyList);
				} else {
					recordId = entity.Property(keyName).OriginalValue.ToString();
				}
			} else {
				recordId = null;
			}

			if(entity.State == EntityState.Modified) {
				foreach(var property in dbEntityType.GetProperties().Where(prop => entity.Property(prop.Name).CurrentValue != entity.Property(prop.Name).OriginalValue)) { 
					var log = new AuditLog {
						UserId = userId,
						EventDate = changeTime,
						EventType = (int)entity.State,
						TableName = tableName,
						RecordId = recordId,
						ColumnName = property.Name,
						NewValue = entity.Property(property.Name).CurrentValue.ToJsonString(),
						OriginalValue = entity.Property(property.Name).OriginalValue.ToJsonString()
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
					NewValue = entity.Members.Select(m => new { Name = m.Metadata.Name, Value = m.CurrentValue }).ToJsonString()
				};
				yield return log;
			}

		}
#else
		public override int SaveChanges() {
			if(!TrackChanges) {
				return base.SaveChanges();
			}
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
#endif
	}
	}