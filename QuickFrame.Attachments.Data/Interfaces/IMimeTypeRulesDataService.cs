using QuickFrame.Attachments.Data.Models;
using QuickFrame.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Attachments.Data.Interfaces
{
    public interface IMimeTypeRulesDataService : IDataService<MimeTypeRule>
    {
		bool IsFileAllowed(string fileName);
    }
}
