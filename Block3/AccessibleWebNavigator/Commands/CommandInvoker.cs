namespace AccessibleWebNavigator.Commands;

public class CommandInvoker
{
    private readonly IEnumerable<ICommand> _commands;

    public CommandInvoker(IEnumerable<ICommand> commands)
    {
        _commands = commands ?? throw new ArgumentNullException(nameof(commands));
    }

    public async Task<string> ExecuteCommandAsync(string commandInput)
    {
        foreach (var command in _commands)
        {
            if (command.CanExecute(commandInput))
            {
                return await command.ExecuteAsync(commandInput);
            }
        }

        return $"Unknown command: {commandInput}";
    }
}