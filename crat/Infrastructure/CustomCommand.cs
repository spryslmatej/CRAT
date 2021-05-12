using System;
using System.Windows.Input;

namespace CRAT.Infrastructure
{
	public class CustomCommand : ICommand
	{

		private readonly Action _action;

		public CustomCommand(Action action)
		{
			_action = action;
		}

		public event EventHandler CanExecuteChanged
		{
			add { /* Not used */}
			remove { /* Not used */}
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			_action.Invoke();
		}
	}
}
