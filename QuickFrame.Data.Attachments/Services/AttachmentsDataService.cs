using ExpressMapper;
using QuickFrame.Data.Attachments.Dtos;
using QuickFrame.Data.Attachments.Interfaces;
using QuickFrame.Data.Attachments.Models;
using QuickFrame.Data.Services;

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace QuickFrame.Data.Attachments.Services {

	[Export]
	public class AttachmentsDataService : DataServiceGuid<AttachmentsContext, Attachment>, IAttachmentsDataService {

		public AttachmentsDataService(AttachmentsContext context)
			: base(context) { }

		public Guid CreateAttachment<TModel>(TModel model) {
			var dbModel = Mapper.Map<TModel, Attachment>(model);
			_dbContext.Attachments.Add(dbModel);
			_dbContext.SaveChanges();
			return dbModel.Id;
		}

		public Guid? FindParent(Guid currentGuid) {
			var current = _dbContext.Attachments.First(obj => obj.Id == currentGuid);
			var model = _dbContext.Attachments
				.Where(obj => obj.FileName.Equals(current.FileName, StringComparison.CurrentCultureIgnoreCase) && obj.Id != currentGuid)
				.OrderBy(obj => obj.UploadDate).FirstOrDefault();
			return model?.Id;
		}

		public Guid CreateRevision(AttachmentCreateRevisionDto model) {
			var current = _dbContext.Attachments.First(obj => obj.Id == model.Id);
			current.ParentId = model.ParentId;
			_dbContext.Entry(current).State = EntityState.Modified;
			_dbContext.SaveChanges();
			return current.Id;
		}

		//protected override IEnumerable<Attachment> GetListCore(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false) {
		//	var query = includeDeleted ? _dbContext.Attachments.Where(o => 1 == 1) : _dbContext.Attachments.Where(o => o.IsDeleted == false);
		//	if(start > 0)
		//		query = query.Skip(start);
		//	if(count > 0)
		//		query = query.Take(count);
		//	return query.ToList();
		//}

		//protected override IEnumerable<TResult> GetListBase<TResult>(int start = 0, int count = 0, string columnName = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false) {
		//	var query = includeDeleted ? _dbContext.Attachments.Where(o => 1 == 1) : _dbContext.Attachments.Where(o => o.IsDeleted == false);
		//	if(start > 0)
		//		query = query.Skip(start);
		//	if(count > 0)
		//		query = query.Take(count);
		//	foreach(var obj in query)
		//		yield return Mapper.Map<Attachment, TResult>(obj);
		//}
	}
}