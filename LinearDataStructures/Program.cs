// See https://aka.ms/new-console-template for more information

using System;

class Program
{
    static void Main()
    {
        // Declare and initialize an array of calculator inputs
        string[] inputHistory = { "5 + 3", "10 - 2", "7 * 4", "20 / 5", "3 ^ 2" };

        // Print the original input history
        Console.WriteLine("Original Calculator Input History:");
        foreach (string input in inputHistory)
        {
            Console.WriteLine(input);
        }

        // Modify an entry (change "10 - 2" to "10 / 2")
        inputHistory[1] = "10 / 2";

        // Print the updated input history
        Console.WriteLine("\nUpdated Calculator Input History:");
        foreach (string input in inputHistory)
        {
            Console.WriteLine(input);
        }


        LinkedList<string> results = new LinkedList<string>();
        results.AddLast("Result: 8");
        results.AddLast("Result: 5");
        results.AddLast("Result: 28");
        results.AddLast("Result: 4");
        results.AddLast("Result: 9");

        // Print the linked list
        Console.WriteLine("Calculation Results:");
        foreach (string result in results)
        {
            Console.WriteLine(result);
        }


        results.Remove("Result: 5");

        // Print the updated linked list
        Console.WriteLine("\nUpdated Calculation Results:");
        foreach (string result in results)
        {
            Console.WriteLine(result);
        }

        Stack<string> undoHistory = new Stack<string>();
        undoHistory.Push("Entered 5 + 3");
        undoHistory.Push("Entered 10 - 2");
        undoHistory.Push("Entered 7 * 4");
        undoHistory.Push("Entered 20 / 5");
        undoHistory.Push("Entered 3 ^ 2");

        // Print the stack
        Console.WriteLine("Undo History:");
        foreach (string action in undoHistory)
        {
            Console.WriteLine(action);
        }

        // Pop an element
        Console.WriteLine("\nUndoing Action: " + undoHistory.Pop());
        // Print the updated stack
        Console.WriteLine("\nUpdated Undo History:");
        foreach (string action in undoHistory)
        {
            Console.WriteLine(action);
        }

        // Declare and initialize a queue
        Queue<string> pendingCalculations = new Queue<string>();
        pendingCalculations.Enqueue("Calculate: 15 + 5");
        pendingCalculations.Enqueue("Calculate: 12 - 3");
        pendingCalculations.Enqueue("Calculate: 9 * 2");
        pendingCalculations.Enqueue("Calculate: 16 / 4");
        pendingCalculations.Enqueue("Calculate: 2 ^ 3");

        // Print the queue
        Console.WriteLine("Pending Calculations:");
        foreach (string calculation in pendingCalculations)
        {
            Console.WriteLine(calculation);
        }

        // Dequeue an element
        Console.WriteLine("\nProcessing Calculation: " + pendingCalculations.Dequeue());

        // Print the updated queue
        Console.WriteLine("\nUpdated Pending Calculations:");
        foreach (string calculation in pendingCalculations)
        {
            Console.WriteLine(calculation);
        }
    }
}