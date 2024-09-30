using Microsoft.Playwright;
using System.Net;

namespace AccessibleWebNavigator.Services;

public class WebNavigator : IWebNavigator
{
    private readonly IPage _page;

    public WebNavigator(IPage page)
    {
        _page = page ?? throw new ArgumentNullException(nameof(page));
    }

    public async Task<bool> NavigateToAsync(string url)
    {
        try
        {
            var response = await _page.GotoAsync(url);
            return response == null || response.Status == (int)HttpStatusCode.OK;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error navigating to {url}: {ex.Message}");
            return false;
        }
    }

    public async Task<string> GetInnerTextContentAsync()
    {
        try
        {
            return await _page.InnerTextAsync("body");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error getting page content: {ex.Message}");
            return string.Empty;
        }
    }
}