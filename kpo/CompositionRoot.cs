
using System;
using Microsoft.Extensions.DependencyInjection;

public static class CompositionRoot
{
    public static IServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection();

        services.AddSingleton<IInspectionService, ServiceCenter>();
        services.AddSingleton<FleetService>();
        services.AddTransient<ConsoleUI>();
        services.AddTransient<VehicleInput>();
        services.AddTransient<InventoryInput>();

        return services.BuildServiceProvider();
    }
}
