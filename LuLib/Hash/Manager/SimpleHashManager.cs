using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using LuLib.Hash.Model;

namespace LuLib.Hash.Manager
{
	public class SimpleHashManager : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		int maxResults;

		List<SimpleHashFile> computedFiles;
		public IList<SimpleHashFile> ComputedFiles { get { return computedFiles.AsReadOnly(); } }

		public bool ComputationInProgress { get; private set; }
		public SimpleHashManager() : this(50) {}

		public SimpleHashManager(int fileHistoryCount)
		{
			if (fileHistoryCount <= 0)
			{
				fileHistoryCount = 50;
			}

			ComputationInProgress = false;
			maxResults = fileHistoryCount;
			computedFiles = new List<SimpleHashFile>(maxResults);
		}

		public async Task<bool> ComputeHash(string filePath)
		{
			SimpleHashFile newFile = new SimpleHashFile(filePath);

			if (newFile.FileExists())
			{
				ComputationInProgress = true;

				await newFile.ComputeFileHash();

				if (computedFiles.Count == maxResults)
				{
					computedFiles.RemoveAt(0);
				}
				computedFiles.Add(newFile);

				ComputationInProgress = false;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ComputedFiles"));
				return true;
			}
			
			ComputationInProgress = false;
			return false;
		}
	}
}
