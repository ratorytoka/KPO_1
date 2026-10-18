public class FakeInspectionService : IInspectionService
{
    private readonly bool Flag;
    public FakeInspectionService(bool flag)
    {
        Flag = flag;
    }
    public bool Inspect(Vehicle vehicle)
    {
        return Flag;
    }
}