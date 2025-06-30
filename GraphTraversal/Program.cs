using System;
using System.Collections.Generic;
class Graph
{
    private Dictionary<int, List<int>> adjacencyList;
    public Graph()
    {
        adjacencyList = new Dictionary<int, List<int>>();
    }
    // Add an edge to the graph
    public void AddEdge(int source, int destination)
    {
        if (!adjacencyList.ContainsKey(source))
        {
            adjacencyList[source] = new List<int>();
        }
        adjacencyList[source].Add(destination);
        if (!adjacencyList.ContainsKey(destination))
        {
            adjacencyList[destination] = new List<int>();
        }
        adjacencyList[destination].Add(source); // Undirected graph
    }

    public void DFS(int start)
    {
        HashSet<int> visited = new HashSet<int>();
        DFSRecursive(start, visited);
        Console.WriteLine();
    }
    private void DFSRecursive(int node, HashSet<int> visited)
    {
        if (visited.Contains(node))
            return;
        Console.Write(node + " ");
        visited.Add(node);
        if (adjacencyList.ContainsKey(node))
        {
            foreach (var neighbor in adjacencyList[node])
            {
                DFSRecursive(neighbor, visited);
            }
        }
    }

    public void BFS(int start)
    {
        HashSet<int> visited = new HashSet<int>();
        Queue<int> queue = new Queue<int>();
        queue.Enqueue(start);
        visited.Add(start);
        while (queue.Count > 0)
        {
            int node = queue.Dequeue();
            Console.Write(node + " ");
            if (adjacencyList.ContainsKey(node))
            {
                foreach (var neighbor in adjacencyList[node])
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }
        }
        Console.WriteLine();
    }
}

class Program
{
    static void Main()
    {
        Graph graph = new Graph();
        // Create sample graph
        graph.AddEdge(1, 2);
        graph.AddEdge(1, 3);
        graph.AddEdge(2, 4);
        graph.AddEdge(2, 5);
        graph.AddEdge(3, 6);
        graph.AddEdge(4, 5);
        Console.WriteLine("Depth-First Search:");
        graph.DFS(1); // Expected output: 1 2 4 5 3 6 (or another valid DFS order)
        Console.WriteLine("Breadth-First Search:");
        graph.BFS(1); // Expected output: 1 2 3 4 5 6
    }
}