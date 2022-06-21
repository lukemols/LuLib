using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuLib.FileDialogs
{
	public interface IFileOpener
	{
		public void SetTitle(string title);
		public void SetStartFolder(string folderPath);
		public bool ShowDialog();
		public string GetSelectedFilePath();
	}
}
