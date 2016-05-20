using QuickFrame.Attachments.Data.Models;
using QuickFrame.Data;
using QuickFrame.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Attachments.Data.Interfaces
{
    public interface IAttachmentsDataService : IDataServiceGuid<Attachment>
    {
		Attachment Get(string fileName);
		TResult Get<TResult>(string fileName) where TResult : DataTransferObjectGuid<Attachment, TResult>;
    }
}
