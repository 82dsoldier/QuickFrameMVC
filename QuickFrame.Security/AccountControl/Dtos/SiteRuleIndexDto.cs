using ExpressMapper;
using Microsoft.AspNetCore.Http;
using QuickFrame.Data.Dtos;
using QuickFrame.Security.AccountControl.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuickFrame.Security.AccountControl.Dtos {

	public class SiteRuleIndexDto : DataTransferObject<SiteRule, SiteRuleIndexDto> {
		private IHttpContextAccessor _contextAccessor;

		public string Url { get; set; }
		public int Priority { get; set; }

		[Display(Name = "Is Allow")]
		public bool IsAllow { get; set; }

		[Display(Name = "Match Partial")]
		public bool MatchPartial { get; set; }

		public List<SiteRoleIndexDto> SiteRoles { get; set; }

		public override void Register() {
			Mapper.Register<SiteRule, SiteRuleIndexDto>()
				.Function(dest => dest.Url, src => {
					Uri uri = null;
					try {
						uri = new Uri(src.Url);
					} catch {
						var request = _contextAccessor.HttpContext.Request;
						uri = new Uri(String.Format("{0}{1}", $"{request.Scheme}://{request.Host}", src.Url));
					}
					return uri.ToString();
				});
		}

		public SiteRuleIndexDto(IHttpContextAccessor contextAccessor) {
			_contextAccessor = contextAccessor;
		}

		public SiteRuleIndexDto() {
		}
	}
}