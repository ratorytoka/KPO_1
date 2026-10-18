using System;

public class InventoryInput
{
    // Ввод шлема
    public Helmet ReadHelmet()
    {
        int number = ReadInventoryNumber();
        return new Helmet(number);
    }

    // Ввод наколенников
    public KneePads ReadKneePads()
    {
        int number = ReadInventoryNumber();
        return new KneePads(number);
    }

    // Ввод налокотников
    public ElbowPads ReadElbowPads()
    {
        int number = ReadInventoryNumber();
        return new ElbowPads(number);
    }

    // Ввод инвентарного номера
    private int ReadInventoryNumber()
    {
        while (true)
        {
            Console.Write("Инвентарный номер: ");
            string? input = Console.ReadLine();

            // Ввод закончился
            if (input == null)
                throw new InvalidOperationException("Ввод завершён");

            // Целое число > 0
            if (int.TryParse(input, out int number) && number > 0)
            {
                return number;
            }

            Console.WriteLine("Введите положительное целое число.");
        }
    }
}