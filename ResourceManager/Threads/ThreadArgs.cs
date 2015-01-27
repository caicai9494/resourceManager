using System;
using System.IO;

namespace ResourceManager
{
	public class ThreadArgs
	{
		public DirectoryInfo dir;
		public long memory;
		public ThreadArgs(DirectoryInfo d)
		{
			dir = d;
			memory = 0;
		}
	}
}

