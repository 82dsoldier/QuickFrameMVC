using QuickFrame.Data.Attachments.Interfaces;
using QuickFrame.Data.Attachments.Models;
using QuickFrame.Data.Services;
using QuickFrame.Di;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Data.Attachments.Services
{
	[Export]
    public class FileExtensionsDataService : DataService<AttachmentsContext, FileExtension>, IFileExtensionsDataService
    {
		public bool FilExtensionExists(int id, string name) {
			using(var context = ComponentContainer.Component<AttachmentsContext>()) {
				if(id == 0)
					return context.Component.FileExtensions.Any(ext => ext.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));

				return context.Component.FileExtensions.Any(ext => ext.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase) && ext.Id != id);
			}
		}
	}
}
