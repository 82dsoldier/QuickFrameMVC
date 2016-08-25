using QuickFrame.Mvc.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Mvc.Models
{
    public class TreeModel : ITreeModel
    {
		public List<ITreeNode> Nodes { get; set; }
		public ITreeNode SelectedNode { get; set; }
    }
}