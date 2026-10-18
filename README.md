# ВышКат

Консольное приложение на C# для учёта транспорта и инвентаря. Позволяет добавлять технику после техосмотра, считать количество транспорта и суточное энергопотребление, выводить список для новичков (`EaseOfUse >= 6`) и весь инвентарь с номерами.

## Запуск

Требуется .NET SDK 10. Команды из корня проекта:

```bash
dotnet run --project kpo/kpo.csproj
dotnet test kpo.Tests/kpo.Tests.csproj
```

## Архитектура и SOLID

- **SRP:** `FleetService` отвечает за учёт парка, `ServiceCenter` за техосмотр, `ConsoleUI` за меню, `VehicleInput` и `InventoryInput` за ввод.
- **OCP:** расчёт энергии работает через `IEnergyConsumer`, а учёт объектов через `IInventory`. Для этих операций не нужно перечислять все конкретные типы.
- **LSP:** `Bicycle`, `Scooter` и `ElectricBicycle` используются через базовый `Vehicle`.
- **ISP:** `IInventory`, `IEnergyConsumer` и `IInspectionService` разделены. Обычный велосипед не обязан реализовывать интерфейс потребления энергии.
- **DIP:** `FleetService` получает `IInspectionService` через конструктор, а не создаёт `ServiceCenter` самостоятельно.

## Dependency Injection (DI)

`FleetService` получает `IInspectionService` через конструктор. В `CompositionRoot.cs` DI-контейнер связывает `IInspectionService` с `ServiceCenter` и создаёт необходимые объекты с помощью `Microsoft.Extensions.DependencyInjection`.

В тестах вместо `ServiceCenter` передаётся `FakeInspectionService`, поэтому проверку приёмки можно тестировать отдельно от настоящего техосмотра.

## Ответы на обязательные вопросы

1. **Наиболее явно применён DIP:** `FleetService` зависит от `IInspectionService`. Поэтому в тестах техосмотр можно заменить.
2. **OCP соблюдён не полностью:** при добавлении нового вида транспорта понадобится изменить меню в `ConsoleUI.Run()`. Для небольшого консольного приложения отдельная система команд не нужна.
3. **Расширение:** новый класс транспорта на основе `Vehicle` можно учитывать в `FleetService` без изменения методов приёмки и подсчёта количества. Для ввода нового типа потребуется добавить команду в `ConsoleUI` и метод ввода. Если транспорт потребляет энергию, он должен реализовать `IEnergyConsumer`.
4. **Тесты:** `CheckFleetServiceWithFalseFakeInspectionService` и `CheckFleetServiceWithTrueFakeInspectionService` проверяют решение техосмотра. В них вместо `ServiceCenter` передаётся `FakeInspectionService`, который возвращает заданное значение. `NoVehiclesForBeginners` и `CorrectVehiclesForBeginners` проверяют правило для новичков. Покрытие строк по последнему отчёту — **92,28%**.
5. **Использование ИИ:** ИИ использовался для обсуждения архитектуры и объяснения особенностей C#. Основной код приложения и большинство тестов написаны самостоятельно. Тесты ReadInventoryItems и CheckConsoleMenu — сгенерированы с помощью ИИ. README также скорректирован с помощью ИИ.
