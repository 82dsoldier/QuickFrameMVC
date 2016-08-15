using ExpressMapper;
using QuickFrame.Data.Attachments.Dtos;
using QuickFrame.Data.Attachments.Interfaces;
using QuickFrame.Data.Attachments.Models;
using QuickFrame.Data.Services;
using QuickFrame.Di;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace QuickFrame.Data.Attachments.Services {

	[Export]
	public class AttachmentsDataService : DataServiceGuid<AttachmentsContext, Attachment>, IAttachmentsDataService {

		public Guid CreateAttachment<TModel>(TModel model) {
			using(var context = ComponentContainer.Component<AttachmentsContext>()) {
				var dbModel = Mapper.Map<TModel, Attachment>(model);
				context.Component.Attachments.Add(dbModel);
				context.Component.SaveChanges();
				return dbModel.Id;
			}
		}

		public Guid? FindParent(Guid currentGuid) {
			using(var context = ComponentContainer.Component<AttachmentsContext>()) {
				var current = context.Component.Attachments.First(obj => obj.Id == currentGuid);
				var model = context.Component.Attachments
					.Where(obj => obj.FileName.Equals(current.FileName, StringComparison.CurrentCultureIgnoreCase) && obj.Id != currentGuid)
					.OrderBy(obj => obj.UploadDate).FirstOrDefault();
				return model?.Id;
			}
		}

		public Guid CreateRevision(AttachmentCreateRevisionDto model) {
			using(var context = ComponentContainer.Component<AttachmentsContext>()) {
				var current = context.Component.Attachments.First(obj => obj.Id == model.Id);
				current.ParentId = model.ParentId;
				context.Component.Entry(current).State = EntityState.Modified;
				context.Component.SaveChanges();
				return current.Id;
			}
		}

		protected override IEnumerable<Attachment> GetListBase(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false) {
			using(var contextFactory = ComponentContainer.Component<AttachmentsContext>()) {
				var query = includeDeleted ? contextFactory.Component.Attachments.Where(o => 1 == 1) : contextFactory.Component.Attachments.Where(o => o.IsDeleted == false);
				if(start > 0)
					query = query.Skip(start);
				if(count > 0)
					query = query.Take(count);
				return query.ToList();
			}
		}

		protected override IEnumerable<TResult> GetListBase<TResult>(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false) {
			using(var contextFactory = ComponentContainer.Component<AttachmentsContext>()) {
				var query = includeDeleted ? contextFactory.Component.Attachments.Where(o => 1 == 1) : contextFactory.Component.Attachments.Where(o => o.IsDeleted == false);
				if(start > 0)
					query = query.Skip(start);
				if(count > 0)
					query = query.Take(count);
				foreach(var obj in query)
					yield return Mapper.Map<Attachment, TResult>(obj);
			}
		}
	}
}