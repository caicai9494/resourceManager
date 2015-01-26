using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace multithredpra
{
	class SearchWorkerForm : Form
	{
		private Label _label;
		private Button _searchBtn;
		private Button _stopBtn;
		private Button _reportBtn;

		private ThreadInfo _info;

		public SearchWorkerForm(string folder, string file)
		{
			_info = new ThreadInfo (folder, file);
			InitializeComponent ();
		}

		private void InitializeComponent()
		{
			Size = new Size (500, 400);
			CenterToScreen ();

			_label = new Label ();
			_label.Parent = this;
			_label.Text = "Path";

			_searchBtn = new Button ();
			_searchBtn.Parent = this;
			_searchBtn.Text = "Search";
			_searchBtn.Location = new Point (0, 100);

			_stopBtn = new Button ();
			_stopBtn.Parent = this;
			_stopBtn.Text = "stop";
			_stopBtn.Location = new Point (0, 200);

			
			_reportBtn = new Button ();
			_reportBtn.Parent = this;
			_reportBtn.Text = "report";
			_reportBtn.Location = new Point (0, 300);
		}
	}
	class SearchForm : Form
	{
		private ListBox _lBox;

		private ThreadInfo _threadInfo;

		private SearchControl _searchControl;  

		public SearchForm(string folder, string file)
		{
			_threadInfo = new ThreadInfo(folder, file);

			InitializeComponent ();
		}

		public void FindFolder()
		{
			_searchControl.FindFolder ();
		}

		public void AddItem(FileInfo f)
		{
			//_lBox.Items.Add (f.FullName);
			Action<FileInfo> del = (info) =>{
				_lBox.Items.Add (f.FullName);
			};

			_lBox.Invoke (del, new object[] { f });
		}

		public void ShowSearchForm()
		{
			Thread th = new Thread (SearchThreadMethod);
			th.Start (_threadInfo);

			Show ();
		}

		private void SearchThreadMethod(Object obj)
		{
			_searchControl = new SearchControl ((obj as ThreadInfo), this);
			FindFolder();
			/*
			Action<ThreadInfo> ThreadHandler = (ThreadInfo info) =>
			{
				_searchControl = new SearchControl (info, this);
				FindFolder();
			};

			Invoke (ThreadHandler, new object[] { obj });
			*/
		}

		private void InitializeComponent()
		{
			Size = new Size (600, 400);
			CenterToScreen ();

			_lBox = new ListBox ();
			_lBox.Parent = this;
			//_lBox.Size = new Size (180, 100);
			_lBox.Dock = DockStyle.Fill;
		}
	}
	class MyForm : Form
	{
		Button btn;
		Label lb;

		public MyForm ()
		{
			Size = new Size (100, 100);

			btn = new Button ();
			btn.Parent = this;
			btn.Text = "enter";
			btn.Click+= (object sender, EventArgs e) => {

				//Thread th = new Thread(ThreadMethod);
				Thread th = new Thread(ThreadMethod);
				th.Start(lb.Text);
			};

			lb = new Label ();
			lb.Text = "nono";
			lb.Parent = this;
			lb.Location = new Point (0, 30);

		}
		
	
		private void ThreadMethod(object info)
		{
			Action<string> del = (string s) => {
				btn.Text = s;
			};
			btn.Invoke (del, new object[] {info});
		}

	
		private void ThreadMethod2()
		{
			lb.Text = "thread";
		}

	}
}

