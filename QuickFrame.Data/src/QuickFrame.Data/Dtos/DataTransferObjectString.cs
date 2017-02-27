using QuickFrame.Data.Interfaces.Dtos;
using QuickFrame.Data.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Data.Dtos
{
    public class DataTransferObjectString<TSrc, TDest>
		: DataTransferObject<TSrc, TDest, string>, IDataTransferObjectString
		where TSrc : class, IDataModel<string> {
    }
}
