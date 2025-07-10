using System;
using System.Collections.Generic;

namespace Hangman
{
    public static class Program
    {
        public static void Main()
        {
            string[] hangmanStages = new string[]
            {
                @"
     +---+
     |   |
         |
         |
         |
         |
    =========",
                @"
     +---+
     |   |
     O   |
         |
         |
         |
    =========",
                @"
     +---+
     |   |
     O   |
     |   |
         |
         |
    =========",
                @"
     +---+
     |   |
     O   |
    /|   |
         |
         |
    =========",
                @"
     +---+
     |   |
     O   |
    /|\  |
         |
         |
    =========",
                @"
     +---+
     |   |
     O   |
    /|\  |
    /    |
         |
    =========",
                @"
     +---+
     |   |
     O   |
    /|\  |
    / \  |
         |
    ========="
            };

            string[] words = { "hello", "banana", "computer", "hangman", "programming" };
            Random rand = new Random();
            string word = words[rand.Next(words.Length)];

            int maxLives = hangmanStages.Length;
            int currentLives = maxLives;
            bool win = false;

            List<char> guessedLetters = new List<char>();

            while (currentLives > 0 && !win)
            {
                Console.Clear();
                Console.WriteLine("HANGMAN GAME");
                Console.WriteLine($"Lives: {currentLives}/{maxLives}");

                int wrongGuesses = maxLives - currentLives;
                Console.WriteLine(hangmanStages[wrongGuesses]);


                Console.Write("Word: ");
                foreach (char c in word)
                {
                    if (guessedLetters.Contains(c))
                    {
                        Console.Write($"{c} ");
                    }
                    else
                    {
                        Console.Write("_ ");
                    }
                }

                Console.WriteLine();
                Console.Write("Guess a letter: ");
                string input = Console.ReadLine()?.ToLower();

                if (string.IsNullOrWhiteSpace(input) || input.Length != 1 || !char.IsLetter(input[0]))
                {
                    Console.WriteLine("Please enter a single valid letter!");
                    Console.ReadKey();
                    continue;
                }

                char guess = input[0];

                if (guessedLetters.Contains(guess))
                {
                    Console.WriteLine("You've already guessed that letter!");
                    Console.ReadKey();
                    continue;
                }

                guessedLetters.Add(guess);

                if (word.Contains(guess))
                {
                    Console.WriteLine("✅ Correct guess!");
                }
                else
                {
                    Console.WriteLine("❌ Incorrect guess!");
                    currentLives--;
                }


                bool wordComplete = true;
                foreach (char c in word)
                {
                    if (!guessedLetters.Contains(c))
                    {
                        wordComplete = false;
                        break;
                    }
                }

                win = wordComplete;
                Console.ReadKey();
            }

            Console.Clear();
            Console.WriteLine("HANGMAN GAME");
            Console.WriteLine(hangmanStages[maxLives - currentLives]);

            if (win)
            {
                Console.WriteLine($"🎉 Congratulations! You guessed the word: {word}");
            }
            else
            {
                Console.WriteLine($"💀 You lost! The word was: {word}");
            }
        }
    }
}
