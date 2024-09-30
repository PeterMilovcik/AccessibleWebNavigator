namespace AccessibleWebNavigator.Services;

public interface IOpenAIService
{
    Task<string> SummarizeTextAsync(string textContent);
    Task<string> AnswerQuestionAsync(string textContent, string question);
}