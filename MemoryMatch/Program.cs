using System;
using System.Linq;
using System.Threading;

public static class Program
{
    public static void Main()
    {
        new MemoryMatchGame(2, 2).Start(); // Можна змінити розмір, наприклад (4, 4)
    }

    public class MemoryMatchGame
    {
        private readonly int rows;
        private readonly int cols;
        private readonly char[] cards;
        private readonly string[] displayGrid;
        private bool[] matched;
        private int matches;
        private readonly Random random = new Random();

        public MemoryMatchGame(int rows, int cols)
        {
            if ((rows * cols) % 2 != 0)
                throw new ArgumentException("Grid must contain an even number of cards!");

            this.rows = rows;
            this.cols = cols;

            cards = GenerateCardPairs(rows * cols);
            Shuffle(cards);

            displayGrid = Enumerable.Range(1, rows * cols).Select(n => n.ToString()).ToArray();
            matched = new bool[rows * cols];
            matches = 0;
        }

        public void Start()
        {
            while (matches < cards.Length / 2)
            {
                PrintGrid();

                int first = GetCardChoice("Select first card: ");
                RevealCard(first);
                PrintGrid();

                int second = GetCardChoice("Select second card: ", exclude: first);
                RevealCard(second);
                PrintGrid();

                if (cards[first] == cards[second])
                {
                    Console.WriteLine("✅ Match!");
                    matched[first] = matched[second] = true;
                    matches++;
                }
                else
                {
                    Console.WriteLine("❌ No match!");
                    Thread.Sleep(1000);
                    HideCard(first);
                    HideCard(second);
                }

                Console.Clear();
            }

            Console.WriteLine("🎉 You found all matches! Congratulations!");
        }

        private void PrintGrid()
        {
            Console.WriteLine();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int index = i * cols + j;
                    Console.Write(displayGrid[index].PadRight(4));
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private int GetCardChoice(string prompt, int? exclude = null)
        {
            int choice;
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (!int.TryParse(input, out choice) || choice < 1 || choice > cards.Length)
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                    continue;
                }

                int index = choice - 1;

                if (matched[index])
                {
                    Console.WriteLine("Card already matched. Choose another.");
                    continue;
                }

                if (exclude.HasValue && index == exclude.Value)
                {
                    Console.WriteLine("You already picked this card. Choose a different one.");
                    continue;
                }

                return index;
            }
        }

        private void RevealCard(int index)
        {
            displayGrid[index] = cards[index].ToString();
        }

        private void HideCard(int index)
        {
            displayGrid[index] = (index + 1).ToString();
        }

        private char[] GenerateCardPairs(int total)
        {
            char[] result = new char[total];
            int asciiStart = 65; // A
            for (int i = 0; i < total; i += 2)
            {
                char c = (char)(asciiStart + i / 2);
                result[i] = c;
                result[i + 1] = c;
            }
            return result;
        }

        private void Shuffle<T>(T[] array)
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (array[i], array[j]) = (array[j], array[i]);
            }
        }
    }
}
