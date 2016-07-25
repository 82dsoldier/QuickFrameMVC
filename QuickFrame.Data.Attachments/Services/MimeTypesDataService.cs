using QuickFrame.Data.Attachments.Interfaces;
using QuickFrame.Data.Attachments.Models;
using QuickFrame.Data.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Data.Attachments.Services
{
	[Export]
    public class MimeTypesDataService : DataService<AttachmentsContext, MimeType>, IMimeTypesDataService
    {
    }
}
