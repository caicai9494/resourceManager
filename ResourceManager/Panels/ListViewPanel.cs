using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Threading;
using ResourceManagerController;
using ResourceManager;

namespace ResourceManagerPanel
{
	static class Constants
	{
		public static int CHECKMEM = 0;
		public static int CHECKMEM100 = 1;
		public static int DELETE = 2;
		public static int COPY = 3;
		public static int RENAME = 4;
		public static int MOVE = 5;
	}
	public class ListViewPanel : Panel
	{
		private ContextMenuStrip _contextMenuStrip;
		public ContextMenuStrip GetContextMenuStrip()
		{
			return _contextMenuStrip;
		}
		private StatusBar _statusBar;
		public void UpdateStatusBar(double num)
		{
			_statusBar.Text = "Memory: " + num.ToString() + " MB";
		}

		private ListView _listView;
		public ListView GetListView()
		{
			return _listView;
		}

		private ListViewController _listViewController;
		public ListViewController GetListViewController()
		{
			return _listViewController;
		}

		public ListViewPanel (Form parent)
		{
			this.Parent = parent;

			Size = new Size (this.Parent.Width / 2, this.Parent.Height - 100);
			this.Dock = DockStyle.Right;

			_listView = new ListView ();
			_listView.Parent = this;
			_listView.Dock = DockStyle.Fill;
			_listView.View = View.LargeIcon;

			_listViewController = new ListViewController (this);

			_statusBar = new StatusBar ();
			_statusBar.Parent = this;
			_statusBar.Dock = DockStyle.Bottom;

			_contextMenuStrip = new ContextMenuStrip ();
			_contextMenuStrip.AutoClose = true;

			ToolStripMenuItem itemCheckMem = new ToolStripMenuItem ("&CheckMemory");
			_contextMenuStrip.Items.Add (itemCheckMem);

			ToolStripMenuItem itemCheckMem100 = new ToolStripMenuItem ("&CheckMemory100");
			_contextMenuStrip.Items.Add (itemCheckMem100);

			ToolStripMenuItem itemDelete = new ToolStripMenuItem ("&Delete");
			itemDelete.Enabled = false;
			_contextMenuStrip.Items.Add (itemDelete);

			ToolStripMenuItem itemCopy = new ToolStripMenuItem ("&Copy");
			_contextMenuStrip.Items.Add (itemCopy);
			itemCopy.Enabled = false;

			ToolStripMenuItem itemRename = new ToolStripMenuItem ("&Rename");
			_contextMenuStrip.Items.Add (itemRename);
			itemRename.Enabled = false;

			ToolStripMenuItem itemMove = new ToolStripMenuItem ("&Move");
			_contextMenuStrip.Items.Add (itemMove);
			itemMove.Enabled = false;

			_contextMenuStrip.ItemClicked += (object sender, ToolStripItemClickedEventArgs e) => 
			{
				if(((ContextMenuStrip)sender).Items[Constants.CHECKMEM] == e.ClickedItem)
				{
					ThreadArgs args = new ThreadArgs(_listViewController.CurrentDirectory);
					Thread thread = new Thread(CalculateMemoryThread);
					thread.IsBackground = true;
					thread.Start(args);
					if(thread.ThreadState == System.Threading.ThreadState.Stopped)
						_statusBar.Text = "Finished";
				}
				else if(((ContextMenuStrip)sender).Items[Constants.CHECKMEM100] == e.ClickedItem)
				{
					ListboxForm lform = new ListboxForm(_listViewController.CurrentDirectory);
					lform.Show();
				}
				else if(((ContextMenuStrip)sender).Items[Constants.DELETE] == e.ClickedItem)
				{
					FileController ufile = new FileController(_listViewController.CurrentDirectory.FullName + Path.DirectorySeparatorChar +
					                              (_listViewController.GetListViewItem().Text));
					ufile.Delete();

					_listViewController.Refresh();
				}
				else if(((ContextMenuStrip)sender).Items[Constants.COPY] == e.ClickedItem)
				{
					FolderBrowserDialog fd = new FolderBrowserDialog();
					fd.ShowNewFolderButton = false;
					fd.ShowDialog();

					FileController ufile = new FileController(_listViewController.CurrentDirectory.FullName + Path.DirectorySeparatorChar +
					                              (_listViewController.GetListViewItem().Text));

					ufile.CopyTo(fd.SelectedPath + Path.DirectorySeparatorChar);

					_listViewController.Refresh();
				}
				else if(((ContextMenuStrip)sender).Items[Constants.RENAME] == e.ClickedItem)
				{
					FileController ufile = new FileController(_listViewController.CurrentDirectory.FullName + Path.DirectorySeparatorChar +
					                              (_listViewController.GetListViewItem().Text));

					PopoutForm pForm = new PopoutForm(ufile);
					pForm.Show();

					ufile.Rename(ufile.NewName);

					_listViewController.Refresh();

				}
				else if(((ContextMenuStrip)sender).Items[Constants.MOVE] == e.ClickedItem)
				{	
					FolderBrowserDialog fd = new FolderBrowserDialog();
					fd.ShowNewFolderButton = false;
					fd.ShowDialog();

					FileController ufile = new FileController(_listViewController.CurrentDirectory.FullName + Path.DirectorySeparatorChar +
					                              (_listViewController.GetListViewItem().Text));

					ufile.MoveTo(fd.SelectedPath + Path.DirectorySeparatorChar);

					_listViewController.Refresh();
				}

			};


			_listView.MouseDown += (object sender, MouseEventArgs e) => 
			{
				if(e.Button == MouseButtons.Right)
					_contextMenuStrip.Show(e.Location.X + this.Parent.Width/2, e.Location.Y + 50);

			};

			_listView.ItemSelectionChanged += (object sender, ListViewItemSelectionChangedEventArgs e) => 
			{
				string path = _listViewController.CurrentDirectory.FullName + Path.DirectorySeparatorChar + e.Item.Text;
				try{
				if(!Directory.Exists(path))
					Process.Start(path);
				}catch(Exception ee){
					Console.WriteLine(ee.Message);
				}

				_listViewController.SetListViewItem(e.Item);

				if(_listViewController.GetListViewItem() != null)
				{
					foreach(ToolStripMenuItem item in _contextMenuStrip.Items){
						item.Enabled = true;
					}

				}
			};

		}

		private void CalculateMemoryThread(object obj)
		{
			ThreadArgs args = obj as ThreadArgs;

			lock (typeof(ThreadArgs)) {
				CalculateMemory (args);
			}
		}

		private void CalculateMemory(ThreadArgs args)
		{
			Action<long> StatusBarHandler = (num) =>
			{
				_statusBar.Text = "Memory: " + num.ToString() + " MB (" + args.dir.FullName + ")";
			};
			try{
				foreach (FileInfo file in args.dir.GetFiles()) {
					args.memory += file.Length;
					_statusBar.Invoke (StatusBarHandler, new object[] { args.memory / 1024 / 1024 });
				}

				foreach(DirectoryInfo dir in args.dir.GetDirectories())
				{
					args.dir = dir;
					CalculateMemory (args);
				}

			}catch(Exception e){
				Console.WriteLine (e.Message);
			}
		}
	}
}

