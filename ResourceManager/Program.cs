using System;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace ResourceManager
{
	class Test
	{
		public Test()
		{
		}
		public void t1()
		{
			string path = "/home/linzhe/book1.pdf";
			Process.Start(path);

		}
	}
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Run (new TestForm ());
		}
	}
}
