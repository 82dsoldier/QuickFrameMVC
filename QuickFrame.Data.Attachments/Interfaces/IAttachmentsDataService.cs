using QuickFrame.Data.Attachments.Dtos;
using QuickFrame.Data.Attachments.Models;
using QuickFrame.Data.Interfaces;
using System;

namespace QuickFrame.Data.Attachments.Interfaces {

	public interface IAttachmentsDataService : IDataServiceGuid<Attachment> {

		Guid CreateAttachment<TModel>(TModel model);

		Guid CreateRevision(AttachmentCreateRevisionDto model);

		Guid? FindParent(Guid currentGuid);
	}
}