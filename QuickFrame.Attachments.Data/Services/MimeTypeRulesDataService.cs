using QuickFrame.Attachments.Data.Interfaces;
using QuickFrame.Attachments.Data.Models;
using QuickFrame.Data;
using QuickFrame.Di;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Attachments.Data.Services
{
	[Export]
	public class MimeTypeRulesDataService : DataService<AttachmentsContext, MimeTypeRule>, IMimeTypeRulesDataService {
		public bool IsFileAllowed(string fileName) {
			using (var contextFactory = ComponentContainer.Component<AttachmentsContext>()) {
				var mimeTypes = contextFactory.Component.MimeTypeRules
					.Where(val => val.MimeType.FileExtension.Equals(Path.GetExtension(fileName), StringComparison.CurrentCultureIgnoreCase) || val.MimeType.FileExtension == "*");
				var exclude = mimeTypes.Where(val => val.IncludeType == false);

				if (exclude != null)
					return false;

				var include = mimeTypes.Where(val => val.IncludeType == true);

				if (include != null)
					return true;

				return false;
			}
		}
	}
}
