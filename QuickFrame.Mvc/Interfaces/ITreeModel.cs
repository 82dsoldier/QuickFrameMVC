using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Mvc.Interfaces
{
    public interface ITreeModel
    {
		List<ITreeNode> Nodes { get; set; }
		ITreeNode SelectedNode { get; set; }
	}
}
