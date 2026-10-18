public class ServiceCenter : IInspectionService
{
    // Техосмотр
    public bool Inspect(Vehicle vehicle)
    {
        // Исправен — принимаем
        if (vehicle.IsServiceable) return true;
        return false;
    }
}

public class ConsoleUI
{
    private readonly VehicleInput VehicleInput;
    private readonly InventoryInput InventoryInput;

    // Получаем зависимости для ввода
    public ConsoleUI(VehicleInput curVehicleInput, InventoryInput curInventoryInput)
    {
        this.VehicleInput = curVehicleInput;
        this.InventoryInput = curInventoryInput;
    }

    // Вывод всего инвентаря
    public void PrintInventory(List<IInventory> items)
    {
        // Нет объектов
        if (items.Count == 0)
        {
            Console.WriteLine("Список пуст");
        }
        foreach (IInventory item in items)
        {
            Console.WriteLine($"{item.Name}: {item.InventoryNumber}");
        }
    }

    // Вывод транспорта для новичков
    public void PrintVehiclesForBeginners(List<Vehicle> vehicles)
    {
        // Нет подходящих
        if (vehicles.Count == 0)
        {
            Console.WriteLine("Подходящего транспорта нет");
        }
        foreach (Vehicle vehicle in vehicles)
        {
            Console.WriteLine($"{vehicle.Name}: {vehicle.InventoryNumber}, простота: {vehicle.EaseOfUse}");
        }
    }

    // Результат добавления
    public void PrintAddResult(bool result)
    {
        // Добавление прошло
        if (result)
        {
            Console.WriteLine("Объект добавлен");
        }
        else
        {
            Console.WriteLine("Не удалось добавить: техосмотр не пройден или номер уже занят");
        }
    }

    // Главное меню
    public void Run(FleetService fleet)
    {
        Console.WriteLine("ВышКат");
        while (true)
        {
            Console.WriteLine("0 - Выход");
            Console.WriteLine("1 - Показать инвентарь");
            Console.WriteLine("2 - Добавить велосипед");
            Console.WriteLine("3 - Добавить электросамокат");
            Console.WriteLine("4 - Добавить электровелосипед");
            Console.WriteLine("5 - Добавить шлем");
            Console.WriteLine("6 - Добавить наколенники");
            Console.WriteLine("7 - Добавить налокотники");
            Console.WriteLine("8 - Транспорт для новичков");
            Console.WriteLine("9 - Статистика парка");
            Console.WriteLine("Выберите команду");
            string? command = Console.ReadLine();
            switch (command)
            {
                case "1":
                    this.PrintInventory(fleet.GetAllInventory());
                    break;

                case "2":
                    this.PrintAddResult(fleet.AddNewVehicle(VehicleInput.ReadBicycle()));
                    break;

                case "3":
                    this.PrintAddResult(fleet.AddNewVehicle(VehicleInput.ReadScooter()));
                    break;

                case "4":
                    this.PrintAddResult(fleet.AddNewVehicle(VehicleInput.ReadElectricBicycle()));
                    break;

                case "5":
                    this.PrintAddResult(fleet.AddNewInventoryItem(InventoryInput.ReadHelmet()));
                    break;

                case "6":
                    this.PrintAddResult(fleet.AddNewInventoryItem(InventoryInput.ReadKneePads()));
                    break;

                case "7":
                    this.PrintAddResult(fleet.AddNewInventoryItem(InventoryInput.ReadElbowPads()));
                    break;

                case "8":
                    this.PrintVehiclesForBeginners(fleet.GetVehiclesForBeginners());
                    break;

                case "9":
                    Console.WriteLine($"Всего транспорта: {fleet.GetTotalVehicles()}");
                    Console.WriteLine($"Суточное энергопотребление: {fleet.GetTotalEnergyConsumption()} кВт·ч");
                    break;

                case "0":
                    return;

                // Ввод закончился
                case null:
                    return;

                default:
                    Console.WriteLine("Неизвестная команда");
                    break;
            }
        }
    }
}

public class FleetService
{
    private readonly List<Vehicle> VehiclePark;
    private readonly IInspectionService InspectionService;
    private readonly List<InventoryItem> ItemCollection;

    // Получаем техосмотр и создаём пустой парк
    public FleetService(IInspectionService curInspectionService)
    {
        this.InspectionService = curInspectionService;
        this.VehiclePark = new List<Vehicle>();
        this.ItemCollection = new List<InventoryItem>();
    }

    // Приём транспорта
    public bool AddNewVehicle(Vehicle vehicle)
    {
        // Нет объекта или номер занят
        if (vehicle == null || HasInventoryNumber(vehicle.InventoryNumber))
        {
            return false;
        }
        // Техосмотр пройден
        if (InspectionService.Inspect(vehicle))
        { 
            VehiclePark.Add(vehicle);
            return true;
        }
        return false;
    }

    // Добавление вещи
    public bool AddNewInventoryItem(InventoryItem item)
    {
        // Нет объекта или номер занят
        if (item == null || HasInventoryNumber(item.InventoryNumber))
        {
            return false;
        }
        ItemCollection.Add(item);
        return true;
    }

    // Ищем номер среди транспорта и вещей
    private bool HasInventoryNumber(int number)
    {
        foreach (Vehicle vehicle in VehiclePark)
        {
            // Номер занят транспортом
            if (vehicle.InventoryNumber == number) return true;
        }
        foreach (InventoryItem item in ItemCollection)
        {
            // Номер занят вещью
            if (item.InventoryNumber == number) return true;
        }
        return false;
    }

    // Кол-во транспорта
    public int GetTotalVehicles()
    {
        return VehiclePark.Count;
    }

    // Общий расход энергии за сутки
    public double GetTotalEnergyConsumption()
    {
        double result = 0;
        foreach (Vehicle vehicle in VehiclePark)
        {
            // Учитываем потребителей энергии
            if (vehicle is IEnergyConsumer EnergyConsumer) 
            {
                result += EnergyConsumer.EnergyConsumption; 
            }
        }

        foreach (InventoryItem item in ItemCollection)
        {
            // Вещи тоже могут потреблять энергию
            if (item is IEnergyConsumer EnergyConsumer) 
            {
                result += EnergyConsumer.EnergyConsumption; 
            }
        }
        return result;
    }

    // Отбираем транспорт для новичков
    public List<Vehicle> GetVehiclesForBeginners()
    {
        List<Vehicle> result = new();
        foreach (Vehicle vehicle in VehiclePark)
        {
            // Простота >= 6 и транспорт исправен
            if (vehicle.EaseOfUse >= 6 && vehicle.IsServiceable) 
            {
                result.Add(vehicle); 
            }
        }
        return result;
    }

    // Собираем транспорт и вещи в один список
    public List<IInventory> GetAllInventory()
    {
        List<IInventory> result = new();
        foreach (IInventory vehicle in VehiclePark)
        {
            result.Add(vehicle); 
        }

         foreach (IInventory item in ItemCollection)
        {
            result.Add(item); 
        }

        return result;
    }
}