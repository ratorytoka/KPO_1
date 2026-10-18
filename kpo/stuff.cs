public abstract class InventoryItem : IInventory
{
    public int InventoryNumber { get; }
    public abstract string Name { get; }

    // Общие данные вещей
    protected InventoryItem(int inventoryNum)
    {
        // Номер > 0
        if (inventoryNum <= 0)
            throw new ArgumentOutOfRangeException(nameof(inventoryNum));

        InventoryNumber = inventoryNum;
    }
}

public class Helmet : InventoryItem
{
    public Helmet(int inventoryNum) : base(inventoryNum) {}
    public override string Name => "Шлем";
}
public class KneePads : InventoryItem
{
    public KneePads(int inventoryNum) : base(inventoryNum) {}
    public override string Name => "Наколенники";
}
public class ElbowPads : InventoryItem
{
    public ElbowPads(int inventoryNum) : base(inventoryNum) {}
    public override string Name => "Налокотники";
}