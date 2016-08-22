using ExpressMapper;
using Microsoft.AspNet.Http.Internal;
using QuickFrame.Data;
using QuickFrame.Di;
using QuickFrame.Security.AccountControl.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuickFrame.Security.AccountControl.Data.Dtos {

	public class SiteRuleIndexDto : DataTransferObject<SiteRule, SiteRuleIndexDto> {
		public string Url { get; set; }
		public int Priority { get; set; }
		[Display(Name ="Is Allow")]
		public bool IsAllow { get; set; }
		[Display(Name ="Match Partial")]
		public bool MatchPartial { get; set; }
		public List<SiteRoleIndexDto> SiteRoles { get; set; }
		public override void Register() {
			Mapper.Register<SiteRule, SiteRuleIndexDto>()
				.Function(dest => dest.Url, src => {
					Uri uri = null;
					try {
						uri = new Uri(src.Url);
					} catch {
						HttpContextAccessor contextAccessor = ComponentContainer.Component<HttpContextAccessor>();
						var request = contextAccessor.HttpContext.Request;
						uri = new Uri(String.Format("{0}{1}", $"{request.Scheme}://{request.Host}", src.Url));
					}
					return uri.ToString();
				});
		}
	}
}