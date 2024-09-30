using System.Text.RegularExpressions;
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

    public async Task<string> AnswerQuestionAsync(string textContent, string question)
    {
        var messages = new List<ChatMessage>
        {
            new SystemChatMessage("You are a helpful assistant that answers questions based on the provided web page content."),
            new UserChatMessage(
                $"Based on the following web page content, please answer the question:\n\n" + 
                $"Content:\n{textContent}\n\nQuestion:\n{question}")
        };

        return await GenerateAsync(messages);
    }

    public async Task<string[]> GetPossibleActionsAsync(string htmlContent)
    {
        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(
                "You are a helpful assistant that is able to " + 
                "list available web page interactions based on " + 
                "the provided web page content " + 
                "for visually impaired users."),
            new UserChatMessage(
                "Based on the following HTML content, " + 
                "list the available user actions or interactions in a numbered list. " + 
                "Only list the actions without any additional explanation.\n" + 
                $"Here is the HTML content:\n\n===\n{htmlContent}\n===\n")
        };

		var actionsText = await GenerateAsync(messages);;
        var actions = actionsText.Split('\n').Select(a => Regex.Replace(a, @"^\d+\.\s*", "")).ToArray();
        return actions;
    }

    public async Task<string> GetElementSelectorAsync(string actionDescription, string htmlContent)
    {
        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(
                "You are a helpful assistant that is able to " + 
                "provide the CSS selector based on user's description and the HTML content. " + 
                "The CSS selector should match exactly the element that is described by the user's action description. " + 
				"Only provide the CSS selector and nothing else. "),
            new UserChatMessage(
                "Based on the following HTML content, " + 
                "list the available user actions or interactions in a numbered list. " + 
                "Only list the actions without any additional explanation.\n" + 
                $"Here is the user's action description:\n\n{actionDescription}\n\n" +
                $"Here is the HTML content:\n\n===\n{htmlContent}\n===\n" + 
                "CSS selector:\n")
        };

        var result = await GenerateAsync(messages);
        result = result.Trim('`');
        return result;
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
            Console.Error.WriteLine($"Error generating response: {ex.Message}");
            return string.Empty;
        }
    }
}