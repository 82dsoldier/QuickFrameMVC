using ExpressMapper;
using Microsoft.AspNetCore.Mvc;
using QuickFrame.Di;
using QuickFrame.Security.AccountControl.Data;
using QuickFrame.Security.AccountControl.Data.Dtos;
using QuickFrame.Security.AccountControl.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace QuickFrame.Security.Areas.Security.Controllers {

	[Area("Security")]
	public class RulesController : Controller {

		public IActionResult Index() {
			var ruleList = new List<SiteRuleIndexDto>();
			using(var context = ComponentContainer.Component<SecurityContext>()) {
				foreach(var rule in context.Component.SiteRules.Where(r => !r.IsDeleted))
					ruleList.Add(Mapper.Map<SiteRule, SiteRuleIndexDto>(rule));
			}
			return View(ruleList);
		}
	}
}