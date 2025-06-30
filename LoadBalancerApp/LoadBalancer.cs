using System;
using System.Collections.Generic;
using System.Linq;
public class LoadBalancer
{
    private List<Server> servers = new List<Server>();
    private int lastUsedServer = -1;
    public void RegisterServer(Server server)
    {
        servers.Add(server);
    }
    public Server GetServerRoundRobin()
    {
        lastUsedServer = (lastUsedServer + 1) % servers.Count;
        return servers[lastUsedServer];
    }
    public Server GetServerLeastConnections()
    {
        return servers.OrderBy(s => s.RequestCount).First();
    }
    public Server GetServerIpHashing(string ipAddress)
    {
        int index = Math.Abs(ipAddress.GetHashCode()) % servers.Count;
        return servers[index];
    }
}