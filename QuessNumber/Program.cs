using System;
using System.Collections.Generic;

namespace NumberGuessingGame
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            PlayGame();
        }

        public static void PlayGame()
        {
            Console.Title = "🎯 Number Guessing Game";
            Random random = new Random();

            Console.WriteLine("Welcome to the Number Guessing Game!");
            Console.WriteLine("Choose difficulty: 1. Easy (1–10)  2. Medium (1–50)  3. Hard (1–100)");
            Console.Write("Your choice: ");
            int max = 10;

            string? difficulty = Console.ReadLine();
            if (difficulty == "2") max = 50;
            else if (difficulty == "3") max = 100;

            int numberToGuess = random.Next(1, max + 1);
            int attempts = 0;
            List<int> history = new();

            Console.WriteLine($"\n🎲 I've picked a number between 1 and {max}. Try to guess it!\n");

            while (true)
            {
                Console.Write("Your guess: ");
                string? input = Console.ReadLine();
                if (!int.TryParse(input, out int guess))
                {
                    Console.WriteLine("⚠️ Invalid input. Please enter a number.");
                    continue;
                }

                if (guess < 1 || guess > max)
                {
                    Console.WriteLine($"❗ Please enter a number between 1 and {max}.");
                    continue;
                }

                attempts++;
                history.Add(guess);

                if (guess > numberToGuess)
                {
                    Console.WriteLine("🔻 Too high!");
                }
                else if (guess < numberToGuess)
                {
                    Console.WriteLine("🔺 Too low!");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n🎉 Correct! You guessed the number in {attempts} tries!");
                    Console.ResetColor();
                    break;
                }
            }

            Console.WriteLine("\n📝 Your guesses: " + string.Join(", ", history));

            Console.Write("\nDo you want to play again? (y/n): ");
            string? again = Console.ReadLine()?.ToLower();
            if (again == "y")
            {
                Console.Clear();
                PlayGame();
            }
            else
            {
                Console.WriteLine("Thanks for playing! 👋");
            }
        }
    }
}
