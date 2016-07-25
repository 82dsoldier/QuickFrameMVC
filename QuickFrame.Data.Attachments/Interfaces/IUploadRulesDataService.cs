﻿using QuickFrame.Data.Attachments.Dtos;
using QuickFrame.Data.Attachments.Models;
using QuickFrame.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Data.Attachments.Interfaces
{
    public interface IUploadRulesDataService : IDataService<UploadRule>
    {
		int GetLastPriority();
		IEnumerable<UploadRuleDto> GetUploadRules();
    }
}
