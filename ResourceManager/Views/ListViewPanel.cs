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
	public class ListViewPanel : Panel
	{
		private ContextMenuStrip _contextMenuStrip;
		public ContextMenuStrip GetContextMenuStrip()
		{
			return _contextMenuStrip;
		}

		private InfoStatusBar _infoStatusBar;

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
			_listView.LabelEdit = true;

			_listViewController = new ListViewController (this);

			_infoStatusBar = new InfoStatusBar (this);

			//_contextMenuStrip = new ContextMenuStrip ();
			//_contextMenuStrip = _listView.ContextMenuStrip;
			_listView.ContextMenuStrip = new ContextMenuStrip ();
			_contextMenuStrip = _listView.ContextMenuStrip;
			//_contextMenuStrip.AutoClose = true;

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

			_listView.MouseClick += (object sender, MouseEventArgs e) => 
			{
				if(e.Button == MouseButtons.Right)
				{
					ListView selectedView = (ListView)sender;

					if(selectedView.SelectedItems.Count > 0)
					{
						_contextMenuStrip.Items[Constants.MOVE].Enabled = true;
						_contextMenuStrip.Items[Constants.DELETE].Enabled = true;
						_contextMenuStrip.Items[Constants.COPY].Enabled = true;
						_contextMenuStrip.Items[Constants.RENAME].Enabled = true;
						selectedView.SelectedItems.Clear();
					}
					else
					{
						_contextMenuStrip.Items[Constants.CHECKMEM].Enabled = true;
						_contextMenuStrip.Items[Constants.CHECKMEM100].Enabled = true;
						_contextMenuStrip.Items[Constants.MOVE].Enabled = false;
						_contextMenuStrip.Items[Constants.DELETE].Enabled = false;
						_contextMenuStrip.Items[Constants.COPY].Enabled = false;
						_contextMenuStrip.Items[Constants.RENAME].Enabled = false;
					}
				}
			};

			_contextMenuStrip.ItemClicked += (object sender, ToolStripItemClickedEventArgs e) => 
			{
				if(((ContextMenuStrip)sender).Items[Constants.CHECKMEM] == e.ClickedItem)
				{
					ThreadArgs args = new ThreadArgs(_listViewController.CurrentDirectory);
					Thread thread = new Thread(CalculateMemoryThread);
					thread.IsBackground = true;
					thread.Start(args);
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

			_listView.AfterLabelEdit += (object sender, LabelEditEventArgs e) => 
			{
			/*	
				FileController ufile = new FileController(_listViewController.CurrentDirectory.FullName + 
				                                          Path.DirectorySeparatorChar + e.Label);
				string newName = e.Label;
				newName = newName.Replace(Environment.NewLine, "");


				ufile.Rename(newName);
*/
			};

			//double click event handler
			_listView.MouseDoubleClick += (object sender, MouseEventArgs e) => 
			{
				ListView selectedView = (ListView)sender;
				if(selectedView.SelectedItems.Count > 0)
				{
					ListViewItem selected = selectedView.SelectedItems[0];

					string path = _listViewController.CurrentDirectory.FullName + Path.DirectorySeparatorChar + selected.Text;
					try{
						if(!Directory.Exists(path))
						 	Process.Start(path);
						else{

						}
					}catch(Exception ee){
						Console.WriteLine(ee.Message);
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
				_infoStatusBar.GetStatusBarController().UpdateStatusBar(num, args.dir.FullName);
			};
			Action StatusBarHandler2 = () =>
			{
				_infoStatusBar.GetStatusBarController().UpdateStatusBar();
			};

			try{
				foreach (FileInfo file in args.dir.GetFiles()) {
					args.memory += file.Length;
					_infoStatusBar.Invoke (StatusBarHandler, new object[] { args.memory / Constants.MB / Constants.MB });
				}

				foreach(DirectoryInfo dir in args.dir.GetDirectories())
				{
					args.dir = dir;
					CalculateMemory (args);
				}

			}catch(Exception e){
				Console.WriteLine (e.Message);
			}

			_infoStatusBar.Invoke (StatusBarHandler2);
				
		}
	}
}

