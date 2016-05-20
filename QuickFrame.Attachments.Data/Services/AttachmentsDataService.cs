using ExpressMapper;
using QuickFrame.Attachments.Data.Interfaces;
using QuickFrame.Attachments.Data.Models;
using QuickFrame.Data;
using QuickFrame.Di;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Attachments.Data.Services
{
	[Export]
	public class AttachmentsDataService : DataServiceGuid<AttachmentsContext, Attachment>, IAttachmentsDataService {
		public override Attachment Get(Guid id) {
			Attachment attachment = base.Get(id);
			if(AttachmentsContext.UseFilestream)
				GetAttachmentData(attachment);
			return attachment;
		}

		public Attachment Get(string fileName) {
			using (var contextFactory = ComponentContainer.Component<AttachmentsContext>()) {
				Attachment attachment = contextFactory.Component.Attachments.FirstOrDefault(att => att.Name.Equals(fileName, StringComparison.CurrentCultureIgnoreCase));
				if(attachment != null && AttachmentsContext.UseFilestream)
					GetAttachmentData(attachment);
				return attachment;
			}
		}

		public TResult Get<TResult>(string fileName) where TResult : DataTransferObjectGuid<Attachment, TResult> => Mapper.Map<Attachment, TResult>(Get(fileName));
		public override void Save(Attachment model) {
			base.Save(model);
			if(AttachmentsContext.UseFilestream)
				SaveAttachmentData(model);
		}

		private void GetAttachmentData(Attachment entity) {
			using (var contextFactory = ComponentContainer.Component<AttachmentsContext>()) {
				var rowData = contextFactory.Component.Database.SqlQuery<FileStreamRowData>(
					"SELECT Data.PathName() AS 'Path', GET_FILESTREAM_TRANSACTION_CONTEXT() AS 'Transaction' FROM Attachments WHERE Id=@id",
					new SqlParameter("id", entity.Id)).First();

				using (var source = new SqlFileStream(rowData.Path, rowData.Transaction, FileAccess.Read)) {
					using (MemoryStream ms = new MemoryStream()) {
						var buffer = new byte[10240];
						int bytesRead = 0;
						while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0) {
							ms.Write(buffer, 0, bytesRead);
						}
						entity.Data = ms.ToArray();
					}
				}
			}
		}

		private void SaveAttachmentData(Attachment entity) {
			using (var contextFactory = ComponentContainer.Component<AttachmentsContext>()) {
				var rowData = contextFactory.Component.Database.SqlQuery<FileStreamRowData>(
				"SELECT Data.PathName() AS 'Path', GET_FILESTREAM_TRANSACTION_CONTEXT() AS 'Transaction' FROM Attachments WHERE Id=@id",
				new SqlParameter("id", entity.Id)).First();

				using (var dest = new SqlFileStream(rowData.Path, rowData.Transaction, FileAccess.Write)) {
					var buffer = new byte[10240];
					int bytesRead = 0;
					using (MemoryStream ms = new MemoryStream(entity.Data)) {
						while ((bytesRead = ms.Read(buffer, 0, buffer.Length)) > 0) {
							dest.Write(buffer, 0, bytesRead);
						}
					}
				}
			}
		}
	}
}
