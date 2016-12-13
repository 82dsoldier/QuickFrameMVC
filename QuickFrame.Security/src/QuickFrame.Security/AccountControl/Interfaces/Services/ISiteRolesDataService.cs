using QuickFrame.Security.AccountControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Security.AccountControl.Interfaces.Services
{
    public interface ISiteRolesDataService
    {
		void AddUser(UserRole userRole);
		void Create(SiteRole role);
		void Create<TModel>(TModel model);
		void Edit(SiteRole role);
		void Edit<TModel>(TModel model);
		void DeleteUser(string userId, string roleId);
    }
}
