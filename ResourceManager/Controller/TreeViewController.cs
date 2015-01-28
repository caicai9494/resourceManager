using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using ResourceManagerPanel;
using ResourceManager;

namespace ResourceManagerController
{
	public class TreeViewController
	{
		private TreeViewPanel _treeViewPanel;
		public TreeViewController (TreeViewPanel tv)
		{
			_treeViewPanel = tv;
			Initialize ();
		}

		private void Initialize()
		{

			TreeNode n = new TreeNode ();
			n.Text = Constants.ROOT.FullName;
			_treeViewPanel.GetTreeView().Nodes.Add (n);

			AddNode (n, Constants.ROOT);
		}

		public void AddNode(TreeNode tnode, DirectoryInfo dir)
		{
			ClearAllNode (tnode);

			try{
				foreach (DirectoryInfo d in dir.GetDirectories()) {
					//hide directory starts with .
					if(!d.Name.StartsWith("."))
					{
						TreeNode n = new TreeNode ();
						n.Text = d.FullName;
						tnode.Nodes.Add (n);
					}
				}
			}catch{
				Console.WriteLine ("Fair to add note");
				//MessageBox.Show ("Fail to access {1} !", tnode.Name); 
			}
		}

		public void ClearAllNode(TreeNode tnode)
		{
			if (tnode.Nodes.Count != 0)
				tnode.Nodes.Clear ();
		}

		public void SearchNode(DirectoryInfo dir)
		{

		}

		public void RemoteNode(DirectoryInfo dir)
		{
		}

	}
}

