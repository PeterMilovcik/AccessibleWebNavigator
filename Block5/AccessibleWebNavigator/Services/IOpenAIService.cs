namespace AccessibleWebNavigator.Services;

public interface IOpenAIService
{
    Task<string> SummarizeTextAsync(string textContent);
    Task<string> AnswerQuestionAsync(string textContent, string question);
    Task<string[]> GetPossibleActionsAsync(string htmlContent);
    Task<string> GetElementSelectorAsync(string actionDescription, string htmlContent);
}