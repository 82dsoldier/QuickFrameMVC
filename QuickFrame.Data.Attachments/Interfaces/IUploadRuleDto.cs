using Microsoft.AspNetCore.Http;
using QuickFrame.Data.Attachments.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Data.Attachments.Interfaces
{
    public interface IUploadRuleDto
    {
		bool IsMatch(IFormFile file);
	}
}
