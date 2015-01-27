using System;
using System.IO;
using System.Windows.Forms;
using ResourceManagerPanel;

namespace ResourceManagerController
{
	public class ListViewController
	{
		public Action<FileSystemInfo> ListViewPanelHandler;

		private DirectoryInfo _currentDirectory;
		public DirectoryInfo CurrentDirectory
		{
			get{
				return _currentDirectory;
			}
		}

		private ListViewItem _listViewItem;
		public void SetListViewItem(ListViewItem item)
		{
			_listViewItem = item;
		}
		public ListViewItem GetListViewItem()
		{
			return _listViewItem;
		}

		private ListViewPanel _listViewPanel;
		public ListViewController (ListViewPanel listviewpanel)
		{
			_listViewPanel = listviewpanel;
		}


		public void PopulateList(DirectoryInfo dir)
		{
			_currentDirectory = dir;

			try{
					foreach (DirectoryInfo d in dir.GetDirectories()) {
						if(!d.Name.StartsWith("."))
							_listViewPanel.GetListView ().Items.Add (d.Name);
					}
					foreach (FileInfo f in dir.GetFiles()) {
						if(!f.Name.StartsWith("."))
							_listViewPanel.GetListView ().Items.Add (f.Name);
					}

			}catch{
				MessageBox.Show ("Fail to access {1} !", dir.Name); 
				ClearList ();
			}
		}

		public void Refresh()
		{
			ClearList ();
			PopulateList (CurrentDirectory);
			DisableContextMenuItems ();
		}

		public void ClearList()
		{
			_listViewPanel.GetListView ().Items.Clear ();
		}

		public void DisableContextMenuItems()
		{
			foreach (ToolStripItem item in _listViewPanel.GetContextMenuStrip().Items) {
				item.Enabled = false;
			}
			_listViewPanel.GetContextMenuStrip ().Items [0].Enabled = true;
		}

	}
}

