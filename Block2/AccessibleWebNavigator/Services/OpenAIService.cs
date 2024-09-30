using OpenAI.Chat;

namespace AccessibleWebNavigator.Services;

public class OpenAIService : IOpenAIService
{
    private readonly ChatClient _chatClient;

    public OpenAIService(IApiKeyProvider apiKeyProvider)
    {
        var apiKey = apiKeyProvider.GetOpenAiApiKey();
        _chatClient = new ChatClient("gpt-3.5-turbo", apiKey);
        // If you have your own API Key, you can try different models as well, but be aware of the billing costs
        // here are some suggestions: gpt-4, gpt-4-turbo, gpt-4o-mini, o1-mini, o1-preview
        // for more details about models and their parameters, visit: https://platform.openai.com/docs/models
        // if you want to learn more about model pricing, visit: https://openai.com/api/pricing/
    }

    public async Task<string> SummarizeTextAsync(string textContent)
    {
        var messages = new List<ChatMessage>
        {
            new SystemChatMessage("You are a helpful assistant that summarizes web page content."),
            new UserChatMessage($"Please provide a concise summary of the following content:\n\n{textContent}")
        };
        return await GenerateAsync(messages);
    }

    private async Task<string> GenerateAsync(List<ChatMessage> messages)
    {
        try
        {
            var response = await _chatClient.CompleteChatAsync(messages);
            return response.Value.ToString();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred while generating the response: {ex.Message}");
            return $"An error occurred while generating the response. Please try again later.";
        }
    }
}
