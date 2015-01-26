using System;
using System.IO;

namespace ResourceManager
{
	public class FileController : FileSystemInfo
	{
		private string _newName;
		public string NewName{
			set{
				_newName = value;
			}
			get{
				return _newName;
			}
		}
		public override bool Exists {
			get {
				return _directory != null || _file != null;
			}
		}
		public override string Name {
			get {
				if (!string.IsNullOrEmpty (_directory.Name))
					return _directory.Name;
				else if (!string.IsNullOrEmpty (_file.Name))
					return _file.Name;
				else
					return "";
			}
		}
		private DirectoryInfo _directory;
		private FileInfo _file;
		//path to directory or file
		public FileController (string path)
		{
			//Console.WriteLine (path);

			if (File.Exists (path))
				_file = new FileInfo (path);
			else if (Directory.Exists (path))
				_directory = new DirectoryInfo (path);
		}

		//dst and src all ended by slash/
		private void CopyFolder(string dst, string src)
		{

			if (Directory.Exists (dst) && Directory.Exists (src)) {
				string path = dst + new DirectoryInfo (src).Name + Path.DirectorySeparatorChar;
				if (Directory.CreateDirectory (path) != null) {

					foreach (FileInfo f in new DirectoryInfo(src).GetFiles()) {
						f.MoveTo (path + f.Name);
						Console.WriteLine (path + f.Name);
					}

					foreach (DirectoryInfo d in new DirectoryInfo(src).GetDirectories()) {
						CopyFolder(path, d.FullName + Path.DirectorySeparatorChar);
					}

				}
			}
		}

		//path to directory ended by slash/
		public void CopyTo(string path)
		{	
			if (_file != null) {
				path += _file.Name;
				//Console.WriteLine (path);
				if (File.Exists (path)) {
					File.Delete (path);
				}
				_file.CopyTo (path);
			}
			else if(_directory != null){
				//Console.WriteLine (_directory.FullName);
				try{
					CopyFolder (path, _directory.FullName + Path.DirectorySeparatorChar);
				}catch(Exception e){
					Console.WriteLine (e.Message);
				}
			}
		}

		public void MoveTo(string path)
		{
			if (_file != null) {
				_file.CopyTo (path);
				_file.Delete ();
			}
			else if(_directory != null)
			{
				CopyFolder (path, _directory.FullName);
				_directory.Delete (true);
			}
		}

		public override void Delete()
		{
			try{
				if(_file != null)
					_file.Delete();
				else if(_directory != null)
					_directory.Delete(true);
			}catch(Exception e){
				Console.WriteLine (e.Message);
			}
		}

		public void Rename(string name)
		{
			MoveTo (Environment.CurrentDirectory + Path.PathSeparator + name);
		}

	}
}

