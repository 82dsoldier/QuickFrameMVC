using Microsoft.Extensions.Options;
using QuickFrame.Di;
using QuickFrame.Mvc.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Mvc.Models
{
    public class ExplorerModel
    {
		public List<string> BasePaths { get; } = new List<string>();
		public string CurrentPath { get; set; }

		public ExplorerModel() {
			var options = ComponentContainer.Component<IOptions<QuickFrameMvcOptions>>();
			BasePaths.AddRange(options.Component.Value.ExplorerBasePaths);
		}
    }
}