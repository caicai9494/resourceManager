using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace ResourceManager
{
	public class NavigationPanel:Panel
	{
		public Action<TreeNode, DirectoryInfo> AddNodeHandler;
		public Action<TreeNode> RemoveNodeHandler;

		private NavigationController _navigationController;
		public NavigationController GetNavigationController()
		{
			return _navigationController;
		}

		private Button _rightButton;
		public Button GetRightButton()
		{
			return _rightButton;
		}

		private Button _leftButton;
		public Button GetLeftButton()
		{
			return _leftButton;
		}


		public NavigationPanel (Form parent)
		{
			this.Parent = parent;
			this.Size = new Size (50, 50);
			this.Dock = DockStyle.Top;

			_rightButton = new Button ();
			_rightButton.Text = "Go to next";
			_rightButton.Dock = DockStyle.Left;
			_rightButton.Parent = this;
			_rightButton.MouseClick += (object sender, MouseEventArgs e) => 
			{
				if(e.Button == MouseButtons.Left)
				{
					_navigationController.Backward();
				}
			};

			_leftButton = new Button ();
			_leftButton.Text = "Go to previous";
			_leftButton.Dock = DockStyle.Left;
			_leftButton.Parent = this;
			_leftButton.MouseClick += (object sender, MouseEventArgs e) => 
			{
				if(e.Button == MouseButtons.Left)
				{
					_navigationController.Forward();
				}
			};

			_navigationController = new NavigationController (this);
		}

	}
}

