using System;

class Program
{
    static void Main(string[] args)
    {
        string? continueInput;

        do
        {
            int num1, num2;

            // Input for the first number with validation
            while (true)
            {
                Console.Write("Please input first number: ");
                if (int.TryParse(Console.ReadLine(), out num1))
                    break;
                Console.WriteLine("Invalid input. Please enter a valid integer.");
            }

            // Input for the second number with validation
            while (true)
            {
                Console.Write("Please input second number: ");
                if (int.TryParse(Console.ReadLine(), out num2))
                    break;
                Console.WriteLine("Invalid input. Please enter a valid integer.");
            }

            Console.WriteLine("Please choose what you want to do:");
            Console.WriteLine("1. Add");
            Console.WriteLine("2. Minus");
            Console.WriteLine("3. Mult");
            Console.WriteLine("4. Div");

            int choice;

            // Input for operation choice with validation
            while (true)
            {
                Console.Write("Your choice is: ");
                if (int.TryParse(Console.ReadLine(), out choice) && choice >= 1 && choice <= 4)
                    break;
                Console.WriteLine("Invalid choice. Please enter a number between 1 and 4.");
            }

            try
            {
                // Switch based on operation choice
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Your result is: " + Add(num1, num2));
                        break;
                    case 2:
                        Console.WriteLine("Your result is: " + Minus(num1, num2));
                        break;
                    case 3:
                        Console.WriteLine("Your result is: " + Mult(num1, num2));
                        break;
                    case 4:
                        // Check for division by zero
                        Console.WriteLine("Your result is: " + Div(num1, num2));
                        break;
                }
            }
            catch (DivideByZeroException)
            {
                Console.WriteLine("Error: Cannot divide by zero.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }

            // Ask if user wants to continue
            Console.Write("Do you want to continue? (y/n): ");
            continueInput = Console.ReadLine()?.Trim().ToLower();
        }
        while (continueInput == "y");
    }

    public static int Add(int num1, int num2)
    {
        return num1 + num2;
    }

    public static int Minus(int num1, int num2)
    {
        return num1 - num2;
    }

    public static int Mult(int num1, int num2)
    {
        return num1 * num2;
    }

    public static double Div(int num1, int num2)
    {
        if (num2 == 0)
            throw new DivideByZeroException();
        return (double)num1 / num2;
    }
}
