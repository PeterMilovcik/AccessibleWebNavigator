namespace AccessibleWebNavigator.Services;

public interface IOpenAIService
{
    Task<string> SummarizeTextAsync(string textContent);
}