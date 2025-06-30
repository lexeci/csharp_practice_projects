using System;
using System.Collections.Generic;
using System.Diagnostics;

class Program
{
    static int Knapsack(int[] values, int[] weights, int capacity, int n)
    {
        if (n == 0 || capacity == 0)
            return 0;
        if (weights[n - 1] > capacity)
            return Knapsack(values, weights, capacity, n - 1);
        int includeItem = values[n - 1] + Knapsack(values, weights, capacity - weights[n - 1], n - 1);
        int excludeItem = Knapsack(values, weights, capacity, n - 1);
        return Math.Max(includeItem, excludeItem);
    }

    static Dictionary<string, int> memo = new Dictionary<string, int>();

    // Optimized Knapsack with Memoization
    static int KnapsackMemoized(int[] values, int[] weights, int capacity, int n)
    {
        if (n == 0 || capacity == 0)
            return 0;
        string key = n + "|" + capacity;
        if (memo.ContainsKey(key))
            return memo[key];
        if (weights[n - 1] > capacity)
            return memo[key] = KnapsackMemoized(values, weights, capacity, n - 1);
        int includeItem = values[n - 1] + KnapsackMemoized(values, weights, capacity - weights[n - 1], n - 1);
        int excludeItem = KnapsackMemoized(values, weights, capacity, n - 1);
        memo[key] = Math.Max(includeItem, excludeItem);
        return memo[key];
    }


    static void Main()
    {
        int[] values = { 60, 100, 120 };
        int[] weights = { 10, 20, 30 };
        int capacity = 50;
        int n = values.Length;
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        int result1 = Knapsack(values, weights, capacity, n);
        stopwatch.Stop();
        Console.WriteLine("Without Memoization: " + result1 + " (Time: " + stopwatch.ElapsedMilliseconds + "ms)");
        stopwatch.Restart();
        int result2 = KnapsackMemoized(values, weights, capacity, n);
        stopwatch.Stop();
        Console.WriteLine("With Memoization: " + result2 + " (Time: " + stopwatch.ElapsedMilliseconds + "ms)");
    }
}
