using Microsoft.Extensions.DependencyInjection;
using AccessibleWebNavigator.Commands;
using Microsoft.Playwright;

namespace AccessibleWebNavigator.Services
{
    public static class ServiceConfigurator
    {
        public static async Task<ServiceProvider> ConfigureAsync()
        {
            var services = new ServiceCollection();

            // Register services
            services.AddSingleton<IApiKeyProvider, EnvironmentApiKeyProvider>();
            services.AddSingleton<IOpenAIService, OpenAIService>();
            services.AddSingleton<ITextToSpeechService, TextToSpeechService>();
            services.AddSingleton<ISpeechToTextService, SpeechToTextService>();

            // Initialize Playwright, Browser, and Page
            var playwright = await Playwright.CreateAsync();
            var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
            var page = await browser.NewPageAsync();

            // Register IPage as a singleton
            services.AddSingleton(page);

            // Register IWebNavigator, which depends on IPage
            services.AddSingleton<IWebNavigator, WebNavigator>();

            // Register commands
            services.AddSingleton<ICommand, ExitCommand>();
            services.AddSingleton<ICommand, NavigateCommand>();
            services.AddSingleton<ICommand, SummarizeCommand>();
            services.AddSingleton<ICommand, AiCommand>();
            services.AddSingleton<ICommand, ListActionsCommand>();
            services.AddSingleton<ICommand, PerformActionCommand>();

            // Register utilities
            services.AddSingleton<CommandInvoker>();
            services.AddSingleton<VoiceInputHandler>();

            return services.BuildServiceProvider();
        }
    }
}