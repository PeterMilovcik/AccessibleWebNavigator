using Microsoft.Extensions.DependencyInjection;
using AccessibleWebNavigator.Commands;

namespace AccessibleWebNavigator.Services;

public static class ServiceConfigurator
{
    public static ServiceProvider Configure()
    {
        var services = new ServiceCollection();

        services.AddSingleton<ICommand, ExitCommand>();
        services.AddSingleton<CommandInvoker>();

        // Register other services as needed in the future

        return services.BuildServiceProvider();
    }
}