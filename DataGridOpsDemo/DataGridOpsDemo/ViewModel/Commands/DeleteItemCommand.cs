using System;
using System.Windows.Input;

namespace DataGridOpsDemo.ViewModel.Commands
{

    public class DeleteItemCommand : ICommand
    {
        #region Fields

        // Member variables
        private readonly MainWindowViewModel m_ViewModel;

        #endregion

        #region Constructor

        public DeleteItemCommand(MainWindowViewModel viewModel)
        {
            m_ViewModel = viewModel;
        }

        #endregion

        #region ICommand Members

/// <summary>
/// Whether this command can be executed.
/// </summary>
public bool CanExecute(object parameter)
{
    return (m_ViewModel.SelectedItem != null);
}

        /// <summary>
        /// Fires when the CanExecute status of this command changes.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Invokes this command to perform its intended task.
        /// </summary>
        public void Execute(object parameter)
        {
            var selectedItem = m_ViewModel.SelectedItem;
            m_ViewModel.GroceryList.Remove(selectedItem);
        }

        #endregion
    }
}
