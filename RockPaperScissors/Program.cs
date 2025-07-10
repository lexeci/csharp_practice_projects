using System;

namespace RockPaperScissorsGame
{
    public static class Program
    {
        private const int WinningScore = 3;

        public static void Main(string[] args)
        {
            Console.Title = "✊✋✌️ Rock Paper Scissors Game";

            do
            {
                PlayGame();
                Console.WriteLine("\nDo you want to play again? (y/n): ");
            } while (Console.ReadLine()?.Trim().ToLower() == "y");

            Console.WriteLine("Thanks for playing! Goodbye.");
        }

        public static void PlayGame()
        {
            int playerScore = 0;
            int enemyScore = 0;

            Random random = new Random();

            Console.WriteLine("Game started! First to 3 wins.");

            while (playerScore < WinningScore && enemyScore < WinningScore)
            {
                Console.WriteLine($"\nScores - Player: {playerScore} | Enemy: {enemyScore}");

                Console.Write("Enter your choice (R=Rock, P=Paper, S=Scissors): ");
                string? playerInput = Console.ReadLine()?.Trim().ToLower();

                if (playerInput != "r" && playerInput != "p" && playerInput != "s")
                {
                    Console.WriteLine("Invalid input! Please enter R, P, or S.");
                    continue;
                }

                int enemyChoice = random.Next(0, 3);
                string enemyMove = enemyChoice switch
                {
                    0 => "r",
                    1 => "p",
                    _ => "s"
                };

                Console.WriteLine($"Enemy chose: {MoveToWord(enemyMove)}");

                string roundResult = GetRoundResult(playerInput, enemyMove);

                switch (roundResult)
                {
                    case "win":
                        Console.WriteLine("You WIN this round!");
                        playerScore++;
                        break;
                    case "lose":
                        Console.WriteLine("You LOSE this round!");
                        enemyScore++;
                        break;
                    default:
                        Console.WriteLine("This round is a TIE!");
                        break;
                }
            }

            if (playerScore == WinningScore)
                Console.WriteLine("\n🎉 Congratulations! You won the game!");
            else
                Console.WriteLine("\n💀 You lost the game! Better luck next time.");
        }

        private static string MoveToWord(string move) =>
            move switch
            {
                "r" => "Rock",
                "p" => "Paper",
                "s" => "Scissors",
                _ => "Unknown"
            };

        private static string GetRoundResult(string playerMove, string enemyMove)
        {
            if (playerMove == enemyMove)
                return "tie";

            if ((playerMove == "r" && enemyMove == "s") ||
                (playerMove == "p" && enemyMove == "r") ||
                (playerMove == "s" && enemyMove == "p"))
                return "win";

            return "lose";
        }
    }
}
