using QuickFrame.Attachments.Data.Interfaces;
using QuickFrame.Attachments.Data.Models;
using QuickFrame.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Attachments.Data.Services
{
	[Export]
    public class MimeTypesDataService : DataService<AttachmentsContext, MimeType>, IMimeTypesDataService
    {
    }
}
