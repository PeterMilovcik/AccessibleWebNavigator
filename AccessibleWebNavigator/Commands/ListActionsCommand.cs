using AccessibleWebNavigator.Services;

namespace AccessibleWebNavigator.Commands;

public class ListActionsCommand : ICommand
{
    private readonly IWebNavigator _webNavigator;
    private readonly IOpenAIService _openAIService;

    public ListActionsCommand(IWebNavigator webNavigator, IOpenAIService openAIService)
    {
        _webNavigator = webNavigator ?? throw new ArgumentNullException(nameof(webNavigator));
        _openAIService = openAIService ?? throw new ArgumentNullException(nameof(openAIService));
    }

    public bool CanExecute(string commandInput) => 
        commandInput.Trim().ToLower().StartsWith("actions", StringComparison.OrdinalIgnoreCase);

    public async Task<string> ExecuteAsync(string commandInput)
    {
        var htmlContent = await _webNavigator.GetPageHtmlContentAsync();

        if (string.IsNullOrEmpty(htmlContent))
        {
            return "No page content available. Please navigate to a page first.";
        }

        var actions = await _openAIService.GetPossibleActionsAsync(htmlContent);

        if (actions.Length > 0)
        {
            return "Possible actions:\n" + string.Join("\n", actions);
        }
        else
        {
            return "No actions could be determined.";
        }
    }
}