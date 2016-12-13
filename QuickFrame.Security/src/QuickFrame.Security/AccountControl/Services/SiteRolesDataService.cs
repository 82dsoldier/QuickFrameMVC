using QuickFrame.Security.AccountControl.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuickFrame.Security.AccountControl.Models;
using QuickFrame.Security.AccountControl.Data;
using ExpressMapper;
using System.Data.Entity;

namespace QuickFrame.Security.AccountControl.Services
{
	public class SiteRolesDataService : ISiteRolesDataService {
		private SecurityContext _dbContext;

		public SiteRolesDataService(SecurityContext dbContext) {
			_dbContext = dbContext;
		}

		public void AddUser(UserRole userRole) {
			_dbContext.UserRoles.Add(userRole);
			_dbContext.SaveChanges();
		}

		public void Create(SiteRole role) {
			role.Id = role.Name.Replace(" ", "");
			role.NormalizedName = role.Name.ToUpper();
			_dbContext.SiteRoles.Add(role);
			_dbContext.SaveChanges();
		}

		public void Create<TModel>(TModel model) {
			Create(Mapper.Map<TModel, SiteRole>(model));
		}

		public void DeleteUser(string userId, string roleId) {
			var model = _dbContext.UserRoles.First(r => r.RoleId == roleId && r.UserId == userId);
			_dbContext.Entry(model).State = EntityState.Deleted;
			_dbContext.SaveChanges();
		}

		public void Edit(SiteRole role) {
			_dbContext.SiteRoles.Attach(role);
			_dbContext.Entry(role).State = EntityState.Modified;
			_dbContext.SaveChanges();
		}

		public void Edit<TModel>(TModel model) {
			Edit(Mapper.Map<TModel, SiteRole>(model));
		}
	}
}
