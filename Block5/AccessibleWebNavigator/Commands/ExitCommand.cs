namespace AccessibleWebNavigator.Commands;

public class ExitCommand : ICommand
{
    public bool CanExecute(string commandInput) => 
        commandInput.Trim().StartsWith("exit", StringComparison.OrdinalIgnoreCase);

    public Task<string> ExecuteAsync(string commandInput) => 
        Task.FromResult("Exiting application.");
}