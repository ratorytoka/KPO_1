public abstract class Vehicle : IInventory
{
    public int InventoryNumber { get; }
    public int EaseOfUse { get; }
    public bool IsServiceable  { get; private set;}
    public abstract string Name { get; }

    // Общие данные транспорта
    protected Vehicle(int inventoryNum, int ease)
    {
        // Номер >= 0
        if (inventoryNum <= 0)
            throw new ArgumentOutOfRangeException(nameof(inventoryNum));
        // EaseOfUse от 1 до 10
        if (ease < 1 || ease > 10)
            throw new ArgumentOutOfRangeException(nameof(ease));

        InventoryNumber = inventoryNum;
        EaseOfUse = ease;
        IsServiceable = true;
    }

    // Отмечаем поломку
    public void BreakDown()
    {
        IsServiceable = false;
    }

    // Отмечаем ремонт
    public void Repair()
    {
        IsServiceable = true;
    }
}

public class Scooter : Vehicle, IEnergyConsumer
{
    public double EnergyConsumption { get; }

    public Scooter(int inventoryNum, int ease, double energyConsump)
    : base(inventoryNum, ease) // Вызываем конструктор базового класса
    {
        // Отсекаем отрицательные и неконечные значения
        if (!double.IsFinite(energyConsump) || energyConsump < 0)
            throw new ArgumentOutOfRangeException(nameof(energyConsump));

        EnergyConsumption = energyConsump;
    }
    public override string Name => "Электросамокат";
}

public class Bicycle : Vehicle
{
    public Bicycle(int inventoryNum, int ease) : base(inventoryNum, ease) {}
    public override string Name => "Велосипед";
}

public class ElectricBicycle : Bicycle, IEnergyConsumer
{
    public double EnergyConsumption { get; }

    public ElectricBicycle(int inventoryNum, int ease, double energyConsump)
    : base(inventoryNum, ease)
    {
        // Отсекаем отрицательные и неконечные значения
        if (!double.IsFinite(energyConsump) || energyConsump < 0)
            throw new ArgumentOutOfRangeException(nameof(energyConsump));

        EnergyConsumption = energyConsump;
    }
    public override string Name => "Электрический велосипед";
}