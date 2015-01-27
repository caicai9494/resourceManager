using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Threading;
using ResourceManagerPanel;

namespace ResourceManager
{
	class ListboxForm : Form
	{
		private DirectoryInfo _dir;

		private StatusBar _statusbar;
		private ListBox _listbox;
		public ListboxForm(DirectoryInfo d)
		{
			_dir = d;
			InitializeComponent ();
		}

		private void InitializeComponent()
		{
			Size = new Size (400, 400);
			Text = "Files more than 100MB";

			_listbox = new ListBox ();
			_listbox.Parent = this;
			_listbox.Dock = DockStyle.Fill;

			_statusbar = new StatusBar ();
			_statusbar.Parent = this;
			_statusbar.Dock = DockStyle.Bottom;

			this.Load += (object sender, EventArgs e) => 
			{
				Thread thread = new Thread(GetFileMoreThan100);
				thread.Start();
				thread.IsBackground = true;

				if(thread.ThreadState == ThreadState.Stopped)
					_statusbar.Text = "Finished";
			};
		}

		public void PopulateList(string file)
		{
			_listbox.Items.Add (file);
		}
		public void UpdateStatusBar(string file)
		{
			_statusbar.Text = "Searching :" + file;
		}


		public void GetFileMoreThan100(){
			lock (typeof(ListboxForm)) {
				getFileMorethan100 (_dir);
			}
		}
		private void getFileMorethan100(DirectoryInfo dir)
		{
			Action<string> updateListBox = (str) => 
			{
				PopulateList(str);
			};
			Action<string> updateStatusBar = (str) =>
			{
				UpdateStatusBar(str);
			};

			try{
				foreach (FileInfo f in dir.GetFiles()) {
					this.Invoke(updateStatusBar, new object[] {f.Name});
					if (f.Length > 100 * 1024 * 1024)
						this.Invoke (updateListBox, new object[] { f.Name });
				}

				foreach (DirectoryInfo subd in dir.GetDirectories()) {
					getFileMorethan100 (subd);
				}
			}catch{
			}
		}
	}

	class TestForm : Form
	{
		private TreeViewPanel _treeViewPanel;
		private ListViewPanel _listViewPanel;

		public TestForm()
		{
			InitializeComponent ();
		}
		private void InitializeComponent()
		{
			Text = "Resource Manager v1.0";
			Size = new Size (800, 600);
			CenterToScreen ();
			MaximizeBox = false;
			MinimizeBox = false;
			FormBorderStyle = FormBorderStyle.FixedSingle;


			_listViewPanel = new ListViewPanel (this);
			_treeViewPanel = new TreeViewPanel (this);
			_treeViewPanel.TreeViewHandler = _listViewPanel.GetListViewController ().PopulateList;
			_treeViewPanel.TreeViewHandler2 = _listViewPanel.GetListViewController ().ClearList;
		}
	}

	class PopoutForm : Form
	{
		private Label _label;
		private Button _button;
		private TextBox _textbox;

		private FileController _userfile;

		public PopoutForm(FileController uf)
		{
			_userfile = uf;
			InitializeComponent ();
		}

		private void InitializeComponent()
		{
			_label = new Label ();
			_label.Parent = this;
			_label.Text = "Enter the new name";
			_label.Dock = DockStyle.Top;

			_textbox = new TextBox ();
			_textbox.Parent = this;
			_textbox.Dock = DockStyle.Top;

			_button = new Button ();
			_button.Parent = this;
			_button.Text = "SUBMIT";
			_button.Dock = DockStyle.Bottom;
			_button.Click += (object sender, EventArgs e) => {

				_userfile.NewName = _textbox.Text;

				this.Close();

			};
		}

	}
}




