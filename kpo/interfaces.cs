public interface IEnergyConsumer
{
    double EnergyConsumption { get; }
}

public interface IInventory
{
    int InventoryNumber { get; }
    string Name { get; }
}

public interface IInspectionService 
{
    bool Inspect(Vehicle vehicle);
}
