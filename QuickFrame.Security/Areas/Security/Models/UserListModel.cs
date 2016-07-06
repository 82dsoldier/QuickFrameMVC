using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Security.Areas.Security.Models
{
    public class UserListModel
    {
		public string ReturnController { get; set; }
		public string ReturnAction { get; set; }
		public string Filter { get; set; }
		public string UserId { get; set; }
    }
}
