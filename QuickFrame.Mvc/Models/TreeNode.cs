using QuickFrame.Mvc.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Mvc.Models
{
	public abstract class TreeNode : ITreeNode {
		public List<ITreeNode> Children { get; set; }

		public virtual bool HasChildren	{ get { return Children != null && Children.Any(); } }

		public string Id { get; set; }

		public string Name { get; set; }

	}
}
