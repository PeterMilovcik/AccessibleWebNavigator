using Microsoft.Extensions.DependencyInjection;
using AccessibleWebNavigator.Commands;
using AccessibleWebNavigator.Services;

namespace AccessibleWebNavigator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = await ServiceConfigurator.ConfigureAsync();

            var commandInvoker = serviceProvider.GetRequiredService<CommandInvoker>();
            var textToSpeechService = serviceProvider.GetRequiredService<ITextToSpeechService>();
            var voiceInputHandler = serviceProvider.GetRequiredService<VoiceInputHandler>();

            Console.WriteLine("Welcome to Accessible Web Navigator!");
            Console.WriteLine("Type 'exit' to quit the application.");
            Console.WriteLine("You can also use voice commands.");

            bool isRunning = true;

            while (isRunning)
            {
                Console.WriteLine("Please choose input method:");
                Console.WriteLine("1. Type command");
                Console.WriteLine("2. Voice command");
                Console.Write("Enter choice (1 or 2): ");
                var inputChoice = Console.ReadLine();

                string commandInput;

                switch (inputChoice)
                {
                    case "1":
                        Console.Write("Enter text command: ");
                        commandInput = Console.ReadLine() ?? string.Empty;
                        break;
                    case "2":
                        Console.WriteLine("Enter voice command:");
                        commandInput = await voiceInputHandler.GetVoiceCommandAsync();
                        Console.WriteLine($"You said: {commandInput}");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        continue;
                }

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