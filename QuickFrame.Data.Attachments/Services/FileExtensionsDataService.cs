using QuickFrame.Data.Attachments.Interfaces;
using QuickFrame.Data.Attachments.Models;
using QuickFrame.Data.Services;

using System;
using System.ComponentModel.Composition;
using System.Linq;

namespace QuickFrame.Data.Attachments.Services {

	[Export]
	public class FileExtensionsDataService : DataService<AttachmentsContext, FileExtension>, IFileExtensionsDataService {

		public FileExtensionsDataService(AttachmentsContext context) : base(context) {
		}

		public bool FilExtensionExists(int id, string name) {
			if(id == 0)
				return _dbContext.FileExtensions.Any(ext => ext.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));

			return _dbContext.FileExtensions.Any(ext => ext.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase) && ext.Id != id);
		}
	}
}