public class Server
{
    public string Id { get; }
    public int RequestCount { get; private set; }
    public Server(string id)
    {
        Id = id;
        RequestCount = 0;
    }
    public void HandleRequest()
    {
        RequestCount++;
    }
}