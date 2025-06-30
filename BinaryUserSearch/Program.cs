using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Text.Json;
class Program
{
    static int BinarySearch(List<User> sortedUsers, string target)
    {
        int left = 0, right = sortedUsers.Count - 1;
        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            int comparison = sortedUsers[mid].Username.CompareTo(target);
            if (comparison == 0) return mid;
            if (comparison < 0) left = mid + 1;
            else right = mid - 1;
        }
        return -1; // Not found
    }

    static async Task Main()
    {
        List<User> users = await FetchUsersFromAPI();

        Console.WriteLine("\nFetched Users:");
        users.Sort((a, b) => a.Username.CompareTo(b.Username));
        Console.WriteLine("\nSorted Usernames:");
        foreach (var user in users)
        {
            Console.WriteLine(user.Username);
        }

        Console.Write("\nEnter a username to search: ");
        string searchUsername = Console.ReadLine();
        int index = BinarySearch(users, searchUsername);
        if (index != -1)
        {
            Console.WriteLine($"User Found: {users[index].Username} - {users[index].Name}");
        }
        else
        {
            Console.WriteLine("User not found.");
        }
    }
    static async Task<List<User>> FetchUsersFromAPI()
    {
        using HttpClient client = new HttpClient();
        string url = "https://randomuser.me/api/?results=10";
        var response = await client.GetFromJsonAsync<ApiResponse>(url);
        List<User> users = new List<User>();
        foreach (var result in response.Results)
        {
            users.Add(new User
            {
                Username = result.Login.Username,
                Name = $"{result.Name.First} {result.Name.Last}"
            });
        }
        return users;
    }
}
class ApiResponse
{
    public List<ApiResult> Results { get; set; }
}
class ApiResult
{
    public Name Name { get; set; }
    public Login Login { get; set; }
}
class Name
{
    public string First { get; set; }
    public string Last { get; set; }
}
class Login
{
    public string Username { get; set; }
}
class User
{
    public string Username { get; set; }
    public string Name { get; set; }
}
