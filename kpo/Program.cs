
using Microsoft.Extensions.DependencyInjection;

// Создаём DI-контейнер
var provider = CompositionRoot.CreateServiceProvider();

var fleet = provider.GetRequiredService<FleetService>();
var ui = provider.GetRequiredService<ConsoleUI>();

// Запускаем консольное меню
ui.Run(fleet);
