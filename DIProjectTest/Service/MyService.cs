namespace DIProjectTest.Service;

public class MyService : IMyService
{
    private readonly int _serviceId;

    public MyService()
    {
        _serviceId = new Random().Next(10000, 99999);
    }

    public void LogCreation(string message)
    {
        Console.WriteLine($"Service message: {message}, Service id: {_serviceId}");
    }
}