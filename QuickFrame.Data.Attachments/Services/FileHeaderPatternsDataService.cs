using QuickFrame.Data.Attachments.Interfaces;
using QuickFrame.Data.Attachments.Models;
using QuickFrame.Data.Services;
using System.ComponentModel.Composition;

namespace QuickFrame.Data.Attachments.Services {

	[Export]
	public class FileHeaderPatternsDataService : DataService<AttachmentsContext, FileHeaderPattern>, IFileHeaderPatternsDataService {
	}
}