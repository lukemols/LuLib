using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LuLib.Hash.ViewModel;
using SimpleHashProgram.Classes;

namespace SimpleHashProgram
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		WPFFileOpener fileOpener = new WPFFileOpener();

		public MainWindow()
		{
			InitializeComponent();

			SimpleHashViewModel vm = (SimpleHashViewModel)TryFindResource("HashVM");
			if (vm != null)
			{
				vm.FileDialog = fileOpener;
			}
		}
	}
}
