using AccessibleWebNavigator.Services;
using Microsoft.Playwright;

namespace AccessibleWebNavigator.Commands;

public class PerformActionCommand : ICommand
{
    private readonly IWebNavigator _webNavigator;
    private readonly IOpenAIService _openAIService;
    private readonly IPage _page;

    public PerformActionCommand(IWebNavigator webNavigator, IOpenAIService openAIService, IPage page)
    {
        _webNavigator = webNavigator ?? throw new ArgumentNullException(nameof(webNavigator));
        _openAIService = openAIService ?? throw new ArgumentNullException(nameof(openAIService));
        _page = page ?? throw new ArgumentNullException(nameof(page));
    }

    public bool CanExecute(string commandInput)
    {
        string lowerInput = commandInput.Trim().ToLower();
        return lowerInput.StartsWith("click ") || lowerInput.StartsWith("select ");
    }

    public async Task<string> ExecuteAsync(string commandInput)
    {
        var actionDescription = commandInput.Trim();

        var htmlContent = await _webNavigator.GetPageHtmlContentAsync();

        if (string.IsNullOrEmpty(htmlContent))
        {
            return "No page content available. Please navigate to a page first.";
        }

        var selector = await _openAIService.GetElementSelectorAsync(htmlContent, actionDescription);

        if (string.IsNullOrEmpty(selector))
        {
            return "Could not find the element to perform the action.";
        }

        try
        {
            await _page.ClickAsync(selector, new PageClickOptions{ Timeout = 5000 });
            return $"Action '{actionDescription}' performed successfully.";
        }
        catch (Exception ex)
        {
            return $"Failed to perform the action: {ex.Message}";
        }
    }
}
