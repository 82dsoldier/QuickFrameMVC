using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Mvc.Interfaces
{
    public interface ITreeNode
    {
		string Id { get; set; }
		string Name { get; set; }
		List<ITreeNode> Children { get; set; }
		bool HasChildren { get; }
    }
}
