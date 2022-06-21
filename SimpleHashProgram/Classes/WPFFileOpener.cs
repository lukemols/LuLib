using LuLib.FileDialogs;
using Microsoft.Win32;
using System;

namespace SimpleHashProgram.Classes
{
	public class WPFFileOpener : IFileOpener
	{
		string filePath = "";
		string startFolder = "";
		string dialogTitle = "";
		public string GetSelectedFilePath()
		{
			return filePath;
		}

		public void SetStartFolder(string folderPath)
		{
			startFolder = folderPath;
		}

		public void SetTitle(string title)
		{
			dialogTitle = title;
		}

		public bool ShowDialog()
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Title = dialogTitle;
			ofd.InitialDirectory = startFolder;
			ofd.Multiselect = false;
			bool? result = ofd.ShowDialog();
			bool pathFound = result.HasValue && result.Value;
			if (pathFound)
			{
				filePath = ofd.FileName;
			}
			return pathFound;
		}
	}
}
