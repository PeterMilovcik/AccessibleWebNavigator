using AccessibleWebNavigator.Commands;
using AccessibleWebNavigator.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AccessibleWebNavigator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Set up Dependency Injection
            var serviceProvider = await ServiceConfigurator.ConfigureAsync();

            var commandInvoker = serviceProvider.GetRequiredService<CommandInvoker>();
            var textToSpeechService = serviceProvider.GetRequiredService<ITextToSpeechService>();

            Console.WriteLine("Welcome to Accessible Web Navigator!");
            Console.WriteLine("Type 'exit' to quit the application.");

            bool isRunning = true;

            while (isRunning)
            {
                Console.Write("Enter command: ");
                var commandInput = Console.ReadLine() ?? string.Empty;

                var result = await commandInvoker.ExecuteCommandAsync(commandInput);

                Console.WriteLine(result);
                await textToSpeechService.SpeakAsync(result);

                if (commandInput.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    isRunning = false;
                }
            }
        }
    }
}