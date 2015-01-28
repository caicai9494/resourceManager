using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;

namespace ResourceManager
{
	public class NavigationController
	{
		private NavigationPanel _navigationPanel;

		private List<TreeNode> _treeNodeHistory = new List<TreeNode>();
		private int _currentTreeNode = 0;

		public NavigationController (NavigationPanel p)
		{
			_navigationPanel = p;
		}

		public void AddHistoryNode(TreeNode n)
		{
			_treeNodeHistory.Add(n);
			_currentTreeNode ++;

			//debug purposes

			foreach(TreeNode tr in _treeNodeHistory){
				Console.WriteLine (tr.Text);
			}
		}

		public void Forward()
		{
			if (_currentTreeNode > 0) {

				TreeNode trn = _treeNodeHistory [_currentTreeNode - 1];
				if(trn.Nodes.Count > 0)
					trn.Nodes.Clear ();
				DirectoryInfo dir = new DirectoryInfo (trn.Text);
				foreach(DirectoryInfo d in dir.GetDirectories()){
					TreeNode tn = new TreeNode ();
					tn.Text = d.FullName;
					trn.Nodes.Add (tn);
					foreach (DirectoryInfo subd in d.GetDirectories()) {
						TreeNode t = new TreeNode ();
						t.Text = d.FullName;
						tn.Nodes.Add (t);
					
					}
				}
				//_navigationPanel.RemoveNodeHandler (_treeNodeHistory [_currentTreeNode]);
				_currentTreeNode --;
			} else
				;
				//_navigationPanel.GetLeftButton ().Enabled = false;
		}

		public void Backward()
		{
			if (_currentTreeNode >= _treeNodeHistory.Count)
				_navigationPanel.GetRightButton ().Enabled = false;
			else
				_currentTreeNode ++;
		}
	}
}

