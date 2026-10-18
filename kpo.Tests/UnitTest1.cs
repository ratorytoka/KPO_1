
using Xunit;

namespace kpo.Tests;

public class UnitTest1
{
    public UnitTest1()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
    }

    [Fact]
    public void NewFleetIsEmpty()
    {
        ServiceCenter serviceCenter = new();
        FleetService fleet = new FleetService(serviceCenter);
        Assert.Equal(0, fleet.GetTotalVehicles());
    }

    [Fact]
    public void AddingValidVehicleIncreasesCount()
    {
        ServiceCenter serviceCenter = new();
        FleetService fleet = new FleetService(serviceCenter);
        Bicycle bicycle = new Bicycle(1,1);
        bool result = fleet.AddNewVehicle(bicycle);

        Assert.True(result);
        Assert.Equal(1, fleet.GetTotalVehicles());
    }

    [Fact]
    public void NotValidVehicleDontAdd()
    {
        ServiceCenter serviceCenter = new();
        FleetService fleet = new FleetService(serviceCenter);
        Bicycle bicycle = new Bicycle(1,1);
        bicycle.BreakDown();
        bool result = fleet.AddNewVehicle(bicycle);

        Assert.False(result);
        Assert.Equal(0, fleet.GetTotalVehicles());
    }

    [Fact]
    public void CheckFleetServiceWithFalseFakeInspectionService()
    {
        FakeInspectionService serviceCenter = new(false);
        FleetService fleet = new FleetService(serviceCenter);
        Bicycle bicycle = new Bicycle(1,1);
        bool result = fleet.AddNewVehicle(bicycle);

        Assert.False(result);
        Assert.Equal(0, fleet.GetTotalVehicles());
    }

    [Fact]
    public void CheckFleetServiceWithTrueFakeInspectionService()
    {
        FakeInspectionService serviceCenter = new(true);
        FleetService fleet = new FleetService(serviceCenter);
        Bicycle bicycle = new Bicycle(1,1);
        bicycle.BreakDown();
        bool result = fleet.AddNewVehicle(bicycle);

        Assert.True(result);
        Assert.Equal(1, fleet.GetTotalVehicles());
    }

    [Fact]
    public void RepeatInventoryWithTrueFakeInspectionService()
    {
        FakeInspectionService serviceCenter = new(true);
        FleetService fleet = new FleetService(serviceCenter);
        Bicycle bicycle1 = new Bicycle(1,1);
        Bicycle bicycle2 = new Bicycle(1,2);
        fleet.AddNewVehicle(bicycle1);
        bool result = fleet.AddNewVehicle(bicycle2);

        Assert.False(result);
        Assert.Equal(1, fleet.GetTotalVehicles());
    }

    [Fact]
    public void GetTotalEnergyConsumption()
    {
        ServiceCenter serviceCenter = new();
        FleetService fleet = new FleetService(serviceCenter);
        fleet.AddNewVehicle(new Bicycle(1,1));
        fleet.AddNewVehicle(new Scooter(2,1,1.0));
        fleet.AddNewVehicle(new ElectricBicycle(3,2,10.1));
        double result = fleet.GetTotalEnergyConsumption();

        Assert.Equal(11.1, result);
    }

    [Fact]
    public void GetZeroTotalEnergyConsumption()
    {
        ServiceCenter serviceCenter = new();
        FleetService fleet = new FleetService(serviceCenter);
        fleet.AddNewVehicle(new Bicycle(1,1));
        double result = fleet.GetTotalEnergyConsumption();

        Assert.Equal(0, result);
    }

    [Fact]
    public void NoVehiclesForBeginners()
    {
        ServiceCenter serviceCenter = new();
        FleetService fleet = new FleetService(serviceCenter);
        fleet.AddNewVehicle(new Bicycle(1,1));
        fleet.AddNewVehicle(new Scooter(2,1,1.0));
        fleet.AddNewVehicle(new ElectricBicycle(3,2,10.1));

        List<Vehicle> result = fleet.GetVehiclesForBeginners();

        Assert.Empty(result);
    }

    [Fact]
    public void CorrectVehiclesForBeginners()
    {
        ServiceCenter serviceCenter = new();
        FleetService fleet = new FleetService(serviceCenter);
        fleet.AddNewVehicle(new Bicycle(10,5));
        fleet.AddNewVehicle(new Bicycle(1,6));
        fleet.AddNewVehicle(new Bicycle(2,7));
        fleet.AddNewVehicle(new Bicycle(3,8));
        fleet.AddNewVehicle(new Bicycle(4,9));
        fleet.AddNewVehicle(new Bicycle(5,10));

        List<Vehicle> result = fleet.GetVehiclesForBeginners();

        Assert.Equal(5, result.Count);
        Assert.Equal(6, result[0].EaseOfUse);
    }

    [Fact]
    public void AddInventoryItem()
    {
        ServiceCenter serviceCenter = new();
        FleetService fleet = new FleetService(serviceCenter);
        bool result = fleet.AddNewInventoryItem(new Helmet(2));

        Assert.True(result);
    }

    [Fact]
    public void GetInventory()
    {
        ServiceCenter serviceCenter = new();
        FleetService fleet = new FleetService(serviceCenter);
        fleet.AddNewVehicle(new Bicycle(1,6));
        fleet.AddNewInventoryItem(new Helmet(2));

        List<IInventory> result = fleet.GetAllInventory();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void VehicleWithNegativeInventoryNumber()
    {
        bool error = false;

        try
        {
            Bicycle bicycle = new Bicycle(-1, 6);
        }
        catch (ArgumentOutOfRangeException)
        {
            error = true;
        }

        Assert.True(error);
    }

    [Fact]
    public void VehicleWithNegativeEnergy()
    {
        bool error = false;

        try
        {
            Scooter bicycle = new Scooter(1, 6, -1);
        }
        catch (ArgumentOutOfRangeException)
        {
            error = true;
        }

        Assert.True(error);
    }

    [Fact]
    public void VehicleWithNegativeEasy()
    {
        bool error = false;

        try
        {
            Scooter bicycle = new Scooter(1, -1, 1);
        }
        catch (ArgumentOutOfRangeException)
        {
            error = true;
        }

        Assert.True(error);
    }

    [Fact]
    public void RepeatInventoryBetweenVehicleAndItem()
    {
        ServiceCenter serviceCenter = new();
        FleetService fleet = new FleetService(serviceCenter);
        fleet.AddNewVehicle(new Bicycle(1,6));
        bool result = fleet.AddNewInventoryItem(new Helmet(1));

        Assert.False(result);
        Assert.Single(fleet.GetAllInventory());
    }

    [Fact]
    public void VehicleWithTooBigEasy()
    {
        bool error = false;

        try
        {
            Bicycle bicycle = new Bicycle(1,11);
        }
        catch (ArgumentOutOfRangeException)
        {
            error = true;
        }

        Assert.True(error);
    }

    
    [Fact]
    public void CheckDI()
    {
        IServiceProvider provider = CompositionRoot.CreateServiceProvider();

        Assert.NotNull(provider.GetService(typeof(FleetService)));
        Assert.NotNull(provider.GetService(typeof(ConsoleUI)));
        Assert.NotNull(provider.GetService(typeof(VehicleInput)));
        Assert.NotNull(provider.GetService(typeof(InventoryInput)));
    }

    [Fact]
    public void ReadInventoryItems()
    {
        InventoryInput input = new();
        TextReader oldInput = Console.In;

        try
        {
            Console.SetIn(new StringReader("a\n-1\n1\n2\n3\n"));

            Helmet helmet = input.ReadHelmet();
            KneePads kneePads = input.ReadKneePads();
            ElbowPads elbowPads = input.ReadElbowPads();

            Assert.Equal(1, helmet.InventoryNumber);
            Assert.Equal(2, kneePads.InventoryNumber);
            Assert.Equal(3, elbowPads.InventoryNumber);
        }
        finally
        {
            Console.SetIn(oldInput);
        }
    }



    [Fact]
    public void ReadVehicleWithWrongInput()
    {
        VehicleInput input = new();
        TextReader oldInput = Console.In;

        try
        {
            Console.SetIn(new StringReader("abc\n-1\n1\n0\n11\n6\n3\n1\n"));

            Bicycle bicycle = input.ReadBicycle();

            Assert.Equal(1, bicycle.InventoryNumber);
            Assert.Equal(6, bicycle.EaseOfUse);
            Assert.True(bicycle.IsServiceable);
        }
        finally
        {
            Console.SetIn(oldInput);
        }
    }

    [Fact]
    public void ReadVehicleWithWrongEnergy()
    {
        VehicleInput input = new();
        TextReader oldInput = Console.In;

        try
        {
            Console.SetIn(new StringReader("2\n7\na\n-1\n3\n0\n"));

            Scooter scooter = input.ReadScooter();

            Assert.Equal(2, scooter.InventoryNumber);
            Assert.Equal(3, scooter.EnergyConsumption);
            Assert.False(scooter.IsServiceable);
        }
        finally
        {
            Console.SetIn(oldInput);
        }
    }

    [Fact]
    public void CheckConsoleMenu()
    {
        ServiceCenter serviceCenter = new();
        FleetService fleet = new FleetService(serviceCenter);
        ConsoleUI ui = new ConsoleUI(new VehicleInput(), new InventoryInput());
        TextReader oldInput = Console.In;

        try
        {
            string commands =
                "2\n1\n6\n1\n" +
                "3\n2\n7\n2\n1\n" +
                "4\n3\n8\n3\n0\n" +
                "5\n4\n" +
                "6\n5\n" +
                "7\n6\n" +
                "1\n8\n9\nabc\n0\n";

            Console.SetIn(new StringReader(commands));

            ui.Run(fleet);
        }
        finally
        {
            Console.SetIn(oldInput);
        }

        Assert.Equal(2, fleet.GetTotalVehicles());
        Assert.Equal(5, fleet.GetAllInventory().Count);
        Assert.Equal(2, fleet.GetTotalEnergyConsumption());
    }

}
