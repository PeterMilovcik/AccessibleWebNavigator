namespace AccessibleWebNavigator.Services;

public class EnvironmentApiKeyProvider : IApiKeyProvider
{
    public string GetOpenAiApiKey() => 
        Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? string.Empty;
}