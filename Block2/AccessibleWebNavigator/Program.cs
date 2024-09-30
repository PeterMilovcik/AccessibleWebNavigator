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

            Console.WriteLine("Welcome to Accessible Web Navigator!");
            Console.WriteLine("Type 'exit' to quit the application.");

            bool isRunning = true;

            while (isRunning)
            {
                Console.Write("Enter command: ");
                var commandInput = Console.ReadLine() ?? string.Empty;

                var result = await commandInvoker.ExecuteCommandAsync(commandInput);

                Console.WriteLine(result);

                if (commandInput.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    isRunning = false;
                }
            }
        }
    }
}