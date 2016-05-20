using QuickFrame.Attachments.Data.Models;
using QuickFrame.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Attachments.Data.Interfaces
{
    public interface IFileHeaderRulesDataService : IDataService<FileHeaderRule>
    {
		bool IsFileAllowed(byte[] data, string fileExtension = "", int mimeTypeId = 0);
    }
}
