namespace AccessibleWebNavigator.Commands;

public interface ICommand
{
    bool CanExecute(string commandInput);
    Task<string> ExecuteAsync(string commandInput);
}