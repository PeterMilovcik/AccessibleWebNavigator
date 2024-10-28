using AccessibleWebNavigator.Services;

namespace AccessibleWebNavigator.Commands;

public class AiCommand : ICommand
{
    private const string CommandWord = "AI";
    private readonly IWebNavigator _webNavigator;
    private readonly IOpenAIService _openAIService;

    public AiCommand(IWebNavigator webNavigator, IOpenAIService openAIService)
    {
        _webNavigator = webNavigator ?? throw new ArgumentNullException(nameof(webNavigator));
        _openAIService = openAIService ?? throw new ArgumentNullException(nameof(openAIService));
    }

    public bool CanExecute(string commandInput) => 
        commandInput.Trim().ToLower().StartsWith($"{CommandWord}, ", StringComparison.OrdinalIgnoreCase) ||
        commandInput.Trim().ToLower().StartsWith($"{CommandWord} ", StringComparison.OrdinalIgnoreCase);

    public async Task<string> ExecuteAsync(string commandInput)
    {
        var question = commandInput.Trim();

        if (string.IsNullOrEmpty(question))
        {
            return $"Please provide a command for the AI after a command word '{CommandWord}'.";
        }

        var content = await _webNavigator.GetInnerTextContentAsync();

        if (string.IsNullOrEmpty(content))
        {
            return "No page content available. Please navigate to a page first.";
        }

        var answer = await _openAIService.AnswerQuestionAsync(content, question);

        return answer;
    }
}