using System;
using System.Threading.Tasks;
using System.Collections.Generic;
class Program
{

    static async Task HandleMultipleRequestsAsync()
    {
        // Start three asynchronous API requests
        Task<string> task1 = FetchDataAsync("Request 1");
        Task<string> task2 = FetchDataAsync("Request 2");
        Task<string> task3 = FetchDataAsync("Request 3");
        // Wait for all requests to complete
        string[] results = await Task.WhenAll(task1, task2, task3);
        // Print results
        foreach (var result in results)
        {
            Console.WriteLine(result);
        }
    }
    static async Task ProcessBackgroundTasksAsync()
    {
        List<Task> tasks = new List<Task>();
        for (int i = 1; i <= 5; i++)
        {
            tasks.Add(ExecuteTaskAsync(i));
        }
        await Task.WhenAll(tasks);
    }
    static async Task ExecuteTaskAsync(int taskId)
    {
        Console.WriteLine($"Task {taskId} started...");
        await Task.Delay(1000 * taskId); // Simulating different task durations
        Console.WriteLine($"Task {taskId} completed.");
    }

    static async Task Main()
    {
        Console.WriteLine("Starting API request...");
        string result = await FetchDataAsync("Single Request");
        Console.WriteLine(result);

        Console.WriteLine("Starting multiple API requests...");
        await HandleMultipleRequestsAsync();

        // Step 4: Background Task Queue
        Console.WriteLine("Starting background task queue...");
        await ProcessBackgroundTasksAsync();
    }
    static async Task<string> FetchDataAsync(string requestName)
    {
        await Task.Delay(2000); // Simulating API request delay
        return "API request completed.";
    }
}