namespace AccessibleWebNavigator.Services;

public interface IWebNavigator
{
    Task<bool> NavigateToAsync(string url);
    Task<string> GetInnerTextContentAsync();
    Task<string> GetPageHtmlContentAsync();
}