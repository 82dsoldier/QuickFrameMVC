using ExpressMapper;
using QuickFrame.Data.Dtos;
using QuickFrame.Security.Data.Models;
using System;
using System.Data.Entity;

namespace QuickFrame.Security.Data.Dtos {

	public class AuditLogIndexDto : DataTransferObjectLong<AuditLog, AuditLogIndexDto> {
		public string UserId { get; set; }
		public DateTime EventDate { get; set; }
		public string EventType { get; set; }
		public string TableName { get; set; }
		public string RecordId { get; set; }

		public override void Register() {
			Mapper.Register<AuditLog, AuditLogIndexDto>()
				.Function(dest => dest.EventType, src => {
					return Enum.GetName(typeof(EntityState), (EntityState)src.EventType);
				});
		}
	}
}