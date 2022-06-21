using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuLib.Commands;
using LuLib.FileDialogs;
using LuLib.Hash.Manager;
using LuLib.Hash.Model;

namespace LuLib.Hash.ViewModel
{
	public class SimpleHashViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public LuCommand StartComputationCommand { get; set; }
		public LuCommand SelectFileCommand { get; set; }

		public IList<SimpleHashFile> Models { get { return hashManager.ComputedFiles; } }

		public string CurrentFilePath { get { return currentFilePath; } set { currentFilePath = value; } }


		SimpleHashManager hashManager;
		string folderPath;
		string currentFilePath;

		private IFileOpener fileDialog;
		public IFileOpener FileDialog { get { return fileDialog; } set { fileDialog = value; SelectFileCommand.RaiseCanExecuteChanged(); } }

		public SimpleHashViewModel()
		{
			hashManager = new SimpleHashManager();
			hashManager.PropertyChanged += HashManager_PropertyChanged;

			folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			currentFilePath = string.Empty;

			CreateCommands();
		}

		private void HashManager_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
            PropertyChanged?.Invoke(this, e);
		}

		void CreateCommands()
		{
			StartComputationCommand = new LuCommand();
			StartComputationCommand.CanExecuteFunc = CanStartComputationCommand;
			StartComputationCommand.ExecuteFunc = ExecuteStartComputationCommand;

			SelectFileCommand = new LuCommand();
			SelectFileCommand.CanExecuteFunc = CanSelectFileCommand;
			SelectFileCommand.ExecuteFunc = ExecuteSelectFileCommand;
		}

		bool CanStartComputationCommand(object sender)
		{
			return !hashManager.ComputationInProgress && currentFilePath != string.Empty;
		}

		void ExecuteStartComputationCommand(object parameter = null)
		{
			Task.Run(() => hashManager.ComputeHash(currentFilePath));
		}

		bool CanSelectFileCommand(object sender)
		{
			return !hashManager.ComputationInProgress && fileDialog != null;
		}

		void ExecuteSelectFileCommand(object parameter = null)
		{
			if (fileDialog != null)
			{
				fileDialog.SetTitle("Select file to compute hash");
				fileDialog.SetStartFolder(folderPath);

				if (fileDialog.ShowDialog())
				{
					CurrentFilePath = fileDialog.GetSelectedFilePath();
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentFilePath"));
					StartComputationCommand.RaiseCanExecuteChanged();
				}
			}
		}
	}
}
