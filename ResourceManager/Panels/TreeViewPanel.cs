using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using ResourceManagerController;
using ResourceManager;

namespace ResourceManagerPanel 
{
	public class TreeViewPanel :  Panel
	{
		private TreeViewController _treeViewController;
		private TreeView _treeView;

		private List<DirectoryInfo> _directoryHistory = new List<DirectoryInfo> ();

		//addnode
		public Action<DirectoryInfo> TreeViewHandler;
		//clear
		public Action TreeViewHandler2;

		public TreeViewPanel (Form parent)
		{
			this.Parent = parent;

			InitializeComponent ();
		}

		private void InitializeComponent()
		{
			if (this.Parent != null)
				Size = new Size (this.Parent.Width / 2, this.Parent.Height - 100);

			this.Dock = DockStyle.Left;

			_treeView = new TreeView ();
			_treeView.Parent = this;
			_treeView.Dock = DockStyle.Fill;

			_treeViewController = new TreeViewController (_treeView);

			_treeView.AfterExpand += (object sender, TreeViewEventArgs e) => 
			{
				TreeNode clickedNode = e.Node;

				TreeViewHandler2();

				if(TreeViewHandler != null)
					TreeViewHandler(new DirectoryInfo(clickedNode.Text));

				foreach(TreeNode tn in clickedNode.Nodes){
					_treeViewController.AddNode(tn, new DirectoryInfo(tn.Text));
				}

			};

			_treeView.AfterCollapse += (object sender, TreeViewEventArgs e) => 
			{
				TreeNode clickedNode = e.Node;
				foreach(TreeNode tn in clickedNode.Nodes){
					_treeViewController.ClearAllNode(tn);
				}
			};

			_treeView.AfterSelect += (object sender, TreeViewEventArgs e) => 
			{
				TreeNode clickedNode = e.Node;
				TreeViewHandler2();
				TreeViewHandler(new DirectoryInfo(clickedNode.Text));
			};


		}
	}
}

