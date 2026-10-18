using System;

public class VehicleInput
{
    // Ввод целого числа в диапазоне
    private int ReadInt(string message, int min, int max)
    {
        while (true)
        {
            Console.Write(message);
            string? input = Console.ReadLine();

            // Ввод закончился
            if (input == null)
                throw new InvalidOperationException("Ввод завершён");

            // Число норм
            if (int.TryParse(input, out int value) && value >= min && value <= max)
                return value;

            Console.WriteLine("Некорректное число. Попробуйте ещё раз.");
        }
    }

    // Ввод расхода энергии
    private double ReadEnergy()
    {
        while (true)
        {
            Console.Write("Суточное энергопотребление (кВт·ч): ");
            string? input = Console.ReadLine();

            // Ввод закончился
            if (input == null)
                throw new InvalidOperationException("Ввод завершён");

            // Конечное число >= нуля
            if (double.TryParse(input, out double value) && double.IsFinite(value) && value >= 0)
                return value;

            Console.WriteLine("Введите неотрицательное число.");
        }
    }

    // Ввод состояния
    private void ReadState(Vehicle vehicle)
    {
        int state = ReadInt("Транспорт исправен? (1 - да, 0 - нет): ", 0, 1);

        // Если сломан
        if (state == 0)
            vehicle.BreakDown();
    }

    // Ввод велосипеда
    public Bicycle ReadBicycle()
    {
        int number = ReadInt("Инвентарный номер: ", 1, int.MaxValue);
        int ease = ReadInt("Простота использования (1-10): ", 1, 10);

        Bicycle bicycle = new Bicycle(number, ease);
        ReadState(bicycle);
        return bicycle;
    }

    // Ввод электросамоката
    public Scooter ReadScooter()
    {
        int number = ReadInt("Инвентарный номер: ", 1, int.MaxValue);
        int ease = ReadInt("Простота использования (1-10): ", 1, 10);
        double energy = ReadEnergy();

        Scooter scooter = new Scooter(number, ease, energy);
        ReadState(scooter);
        return scooter;
    }

    // Ввод электровелосипеда
    public ElectricBicycle ReadElectricBicycle()
    {
        int number = ReadInt("Инвентарный номер: ", 1, int.MaxValue);
        int ease = ReadInt("Простота использования (1-10): ", 1, 10);
        double energy = ReadEnergy();

        ElectricBicycle bicycle = new ElectricBicycle(number, ease, energy);
        ReadState(bicycle);
        return bicycle;
    }
}