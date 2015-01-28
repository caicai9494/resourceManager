using System;
using System.Windows.Forms;

namespace ResourceManager
{
	public class InfoStatusBar:StatusBar
	{
		private StatusBarController _statusBarController;
		public StatusBarController GetStatusBarController()
		{
			return _statusBarController;
		}

		public InfoStatusBar (Panel p)
		{
			this.Parent = p;
			this.Dock = DockStyle.Bottom;

			_statusBarController = new StatusBarController (this);
		}
	}
}

