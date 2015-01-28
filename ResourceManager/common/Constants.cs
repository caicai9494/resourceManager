using System;
using System.IO;

namespace ResourceManager
{
	static class Constants
	{	
		public static int CHECKMEM = 0;
		public static int CHECKMEM100 = 1;
		public static int DELETE = 2;
		public static int COPY = 3;
		public static int RENAME = 4;
		public static int MOVE = 5;

		public static int MB = 1024;

		public static DirectoryInfo ROOT = new DirectoryInfo("/home/linzhe/Downloads/");
	}
}

