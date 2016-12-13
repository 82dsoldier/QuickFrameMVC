using QuickFrame.Data.Dtos;
using QuickFrame.Security.AccountControl.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Security.AccountControl.Dtos
{
    public class SiteRuleEditDto : DataTransferObject<SiteRule, SiteRuleEditDto>
    {
		public string Url { get; set; }
		public int Priority { get; set; }
		[Display(Name ="Is ALlow")]
		public bool IsAllow { get; set; }
		[Display(Name ="Match Partial")]
		public bool MatchPartial { get; set; }
	}
}
