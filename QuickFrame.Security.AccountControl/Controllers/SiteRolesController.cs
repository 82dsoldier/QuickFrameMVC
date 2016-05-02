using QuickFrame.Mvc;
using QuickFrame.Security.Data.Dtos;
using QuickFrame.Security.Data.Interfaces;
using QuickFrame.Security.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Security.AccountControl.Controllers
{
    public class SiteRolesController : ControllerBase<SiteRole, SiteRoleIndexDto, SiteRoleEditDto>
    {
		public SiteRolesController(ISiteRolesDataService dataService)
			:base(dataService) {

		}
    }
}
