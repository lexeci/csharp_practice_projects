using System;
using System.Linq;
using System.Collections.Generic;

public static class Program
{
    public static void Main()
    {
        TicTacToe game = new TicTacToe();
        game.Run();
    }

    public class TicTacToe
    {
        private int playerWins = 0;
        private int aiWins = 0;
        private int draws = 0;

        public void Run()
        {
            do
            {
                PlayGame();
                Console.Write("\nDo you want to play again? (y/n): ");
            }
            while (Console.ReadLine()?.Trim().ToLower() == "y");

            Console.WriteLine("\n📊 Final Statistics:");
            Console.WriteLine($"Player Wins: {playerWins}");
            Console.WriteLine($"AI Wins:     {aiWins}");
            Console.WriteLine($"Draws:       {draws}");
            Console.WriteLine("\nThanks for playing!");
        }

        private void PlayGame()
        {
            string[] grid = Enumerable.Range(1, 9).Select(i => i.ToString()).ToArray();
            bool isPlayerTurn = true;
            Random rand = new Random();

            while (true)
            {
                PrintGrid(grid);
                if (isPlayerTurn)
                {
                    Console.Write("Player (X), choose your cell (1-9): ");
                    string? input = Console.ReadLine()?.Trim();

                    if (!int.TryParse(input, out int index) || index < 1 || index > 9)
                    {
                        Console.WriteLine("❌ Invalid input. Please enter a number from 1 to 9.");
                        Console.ReadKey();
                        continue;
                    }

                    index--; // convert to 0-based

                    if (grid[index] == "X" || grid[index] == "O")
                    {
                        Console.WriteLine("❌ Cell already taken. Try another.");
                        Console.ReadKey();
                        continue;
                    }

                    grid[index] = "X";
                }
                else
                {
                    Console.WriteLine("🤖 AI (O) is thinking...");
                    System.Threading.Thread.Sleep(500);

                    List<int> availableCells = grid
                        .Select((val, i) => new { val, i })
                        .Where(c => c.val != "X" && c.val != "O")
                        .Select(c => c.i)
                        .ToList();

                    int choice = availableCells[rand.Next(availableCells.Count)];
                    grid[choice] = "O";
                }

                if (CheckVictory(grid))
                {
                    PrintGrid(grid);
                    if (isPlayerTurn)
                    {
                        Console.WriteLine("🎉 Player (X) wins!");
                        playerWins++;
                    }
                    else
                    {
                        Console.WriteLine("💻 AI (O) wins!");
                        aiWins++;
                    }
                    break;
                }

                if (IsDraw(grid))
                {
                    PrintGrid(grid);
                    Console.WriteLine("🤝 It's a draw!");
                    draws++;
                    break;
                }

                isPlayerTurn = !isPlayerTurn;
            }
        }

        private void PrintGrid(string[] grid)
        {
            Console.Clear();
            Console.WriteLine("\n     Tic Tac Toe (Player vs AI)\n");

            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine("     ---+---+---");
                Console.Write("     ");
                for (int j = 0; j < 3; j++)
                {
                    string cell = grid[i * 3 + j];
                    Console.Write($" {cell} ");
                    if (j < 2) Console.Write("|");
                }
                Console.WriteLine();
            }
            Console.WriteLine("     ---+---+---\n");
        }

        private bool CheckVictory(string[] grid)
        {
            int[,] winPositions = new int[,]
            {
                {0,1,2}, {3,4,5}, {6,7,8}, // Rows
                {0,3,6}, {1,4,7}, {2,5,8}, // Columns
                {0,4,8}, {2,4,6}           // Diagonals
            };

            for (int i = 0; i < winPositions.GetLength(0); i++)
            {
                string a = grid[winPositions[i, 0]];
                string b = grid[winPositions[i, 1]];
                string c = grid[winPositions[i, 2]];

                if (a == b && b == c)
                    return true;
            }

            return false;
        }

        private bool IsDraw(string[] grid) => grid.All(cell => cell == "X" || cell == "O");
    }
}
