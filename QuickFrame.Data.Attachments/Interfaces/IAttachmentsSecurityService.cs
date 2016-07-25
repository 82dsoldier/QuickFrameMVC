using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Data.Attachments.Interfaces
{
    public interface IAttachmentsSecurityService
    {
		bool CanFileUpload(IFormFile file);
    }
}
