using System;
using System.Windows.Input;


namespace LuLib.Commands
{
	public class LuCommand : ICommand
	{
        public Predicate<object> CanExecuteFunc
        {
            get;
            set;
        }

        public Action<object> ExecuteFunc
        {
            get;
            set;
        }

        public bool CanExecute(object parameter)
        {
            return CanExecuteFunc(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            ExecuteFunc(parameter);
        }

        public void RaiseCanExecuteChanged()
		{
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}
    }
}
