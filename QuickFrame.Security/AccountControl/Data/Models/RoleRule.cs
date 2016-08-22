using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Security.AccountControl.Data.Models
{
    public class RoleRule
    {
		[ForeignKey("SiteRule")]
		[Key]
		[Column(Order =1)]
		public int RuleId { get; set; }
		[ForeignKey("SiteRole")]
		[Key]
		[Column(Order = 2)]
		public string RoleId { get; set; }

		public virtual SiteRule SiteRule { get; set; }
		public virtual SiteRole SiteRole { get; set; }
    }
}
