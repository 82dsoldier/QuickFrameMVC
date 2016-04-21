using QuickFrame.Data;
using QuickFrame.Security.Data.Interfaces;
using QuickFrame.Security.Data.Models;
using System.ComponentModel.Composition;

namespace QuickFrame.Security.Data.Services {

	[Export]
	public class SiteRolesDataService : DataService<PermissionsContext, SiteRole>, ISiteRolesDataService {
	}
}