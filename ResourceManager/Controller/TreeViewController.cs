using System;
using System.IO;
using System.Windows.Forms;
using ResourceManagerPanel;

namespace ResourceManagerController
{
	public class TreeViewController
	{
		private DirectoryInfo _root = new DirectoryInfo(Directory.GetDirectoryRoot(Environment.CurrentDirectory));
		private TreeView _treeView;
		public TreeViewController (TreeView tv)
		{
			_treeView = tv;
			Initialize ();

			//_directory = new DirectoryInfo ("/");
		}

		private void Initialize()
		{
			//ClearAllNode ();

			TreeNode n = new TreeNode ();
			n.Text = _root.FullName;
			_treeView.Nodes.Add (n);

			AddNode (n, _root);
		}

		public void AddNode(TreeNode tnode, DirectoryInfo dir)
		{
			ClearAllNode (tnode);

			try{
				foreach (DirectoryInfo d in dir.GetDirectories()) {
					TreeNode n = new TreeNode ();
					n.Text = d.FullName;
					tnode.Nodes.Add (n);
				}
			}catch{
				//MessageBox.Show ("Fail to access {1} !", tnode.Name); 
			}
		}

		public void ClearAllNode(TreeNode tnode)
		{
			if (tnode.Nodes.Count != 0)
				tnode.Nodes.Clear ();
		}

	}
}

