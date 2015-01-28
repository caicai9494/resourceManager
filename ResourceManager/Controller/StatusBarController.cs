using System;
using System.Windows.Forms;

namespace ResourceManager
{
	public class StatusBarController
	{
		private double _storage;
		public double GetStorage()
		{
			return _storage;
		}

		private InfoStatusBar _statusBar;
		public StatusBarController (InfoStatusBar sb)
		{
			_statusBar = sb;
		}

		public void UpdateStatusBar(double storage, string dir)
		{
			_storage = storage;
			_statusBar.Text = "Memory: " + storage.ToString () + " MB in " + dir;
		}

		public void UpdateStatusBar()
		{
			_statusBar.Text = "Finished! Memory: " + _storage.ToString () + " MB";
		}
	}
}

