using AccessibleWebNavigator.Services;

namespace AccessibleWebNavigator.Commands;

public class SummarizeCommand : ICommand
{
    private readonly IWebNavigator _webNavigator;
    private readonly IOpenAIService _openAIService;

    public SummarizeCommand(IWebNavigator webNavigator, IOpenAIService openAIService)
    {
        _webNavigator = webNavigator ?? throw new ArgumentNullException(nameof(webNavigator));
        _openAIService = openAIService ?? throw new ArgumentNullException(nameof(openAIService));
    }

    public bool CanExecute(string commandInput)
    {
        var lowerInput = commandInput.Trim().TrimEnd('.').ToLower();
        return 
            lowerInput == "summarize page" || 
            lowerInput == "summarize the page" || 
            lowerInput == "describe page" || 
            lowerInput == "describe the page";
    }

    public async Task<string> ExecuteAsync(string commandInput)
    {
        var content = await _webNavigator.GetInnerTextContentAsync();

        if (string.IsNullOrEmpty(content))
        {
            return "No page content available. Please navigate to a page first.";
        }

        var summary = await _openAIService.SummarizeTextAsync(content);

        return !string.IsNullOrEmpty(summary) ? summary : "Failed to generate summary.";
    }
}