using AccessibleWebNavigator.Services;

namespace AccessibleWebNavigator.Commands;

public class NavigateCommand : ICommand
{
    private readonly IWebNavigator _webNavigator;

    public NavigateCommand(IWebNavigator webNavigator)
    {
        _webNavigator = webNavigator;
    }

    public bool CanExecute(string commandInput)
    {
        var lowerInput = commandInput.Trim().ToLower();
        return lowerInput.StartsWith("navigate to") || lowerInput.StartsWith("go to");
    }

    public async Task<string> ExecuteAsync(string commandInput)
    {
        var urlStartIndex = commandInput.IndexOf("to", StringComparison.OrdinalIgnoreCase) + 2;
        var url = commandInput.Substring(urlStartIndex).Trim();

        if (!url.StartsWith("http", StringComparison.OrdinalIgnoreCase))
        {
            url = "https://" + url;
        }

        Console.WriteLine($"Navigating to {url}...");

        var success = await _webNavigator.NavigateToAsync(url);

        return success ? $"Page '{url}' loaded successfully." : $"Failed to load page.";
    }
}