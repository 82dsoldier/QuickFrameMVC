using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuickFrame.Mvc.Interfaces;
using System.IO;

namespace QuickFrame.Mvc.Models
{
	public class ExplorerNode : TreeNode {
		public override bool HasChildren { get { return File.GetAttributes(Id).HasFlag(FileAttributes.Directory); } }
		public ExplorerNode(string path) {
			Id = path;
			Name = Path.GetFileName(path);

			//if((File.GetAttributes(Id) & FileAttributes.Directory) == FileAttributes.Directory) {
			//	var directoryList = Directory.EnumerateDirectories(Id);

			//	if(directoryList != null) {
			//		Children = new List<ITreeNode>();
			//		foreach(var directory in directoryList)
			//			Children.Add(new ExplorerNode(directory));
			//	}

			//	var fileList = Directory.EnumerateFiles(Id);

			//	if(fileList != null) {
			//		if(Children == null)
			//			Children = new List<ITreeNode>();
			//		foreach(var file in fileList)
			//			Children.Add(new ExplorerNode(file));
			//	}
			//}
		}
	}
}
