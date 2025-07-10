using System;

namespace DiceGame
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "🎲 Dice Game";
            PlayGame();
        }

        public static void PlayGame()
        {
            int playerPoints = 0;
            int aiPoints = 0;
            int rounds = 10;
            Random random = new Random();

            Console.WriteLine("Welcome to the Dice Game!");
            Console.WriteLine($"You will play {rounds} rounds against the AI.\n");

            for (int i = 1; i <= rounds; i++)
            {
                Console.WriteLine($"--- Round {i} ---");
                Console.WriteLine("Press any key to roll your dice...");
                Console.ReadKey(true);

                int playerRoll = random.Next(1, 7);
                int aiRoll = random.Next(1, 7);

                Console.WriteLine($"🎲 You rolled: {playerRoll}");
                Console.WriteLine($"🤖 AI rolled: {aiRoll}");

                if (playerRoll > aiRoll)
                {
                    Console.WriteLine("✅ You won this round!");
                    playerPoints++;
                    if (aiPoints > 0) aiPoints--;
                }
                else if (playerRoll < aiRoll)
                {
                    Console.WriteLine("❌ You lost this round!");
                    if (playerPoints > 0) playerPoints--;
                    aiPoints++;
                }
                else
                {
                    Console.WriteLine("➖ It's a tie!");
                    if (playerPoints > 0) playerPoints--;
                    if (aiPoints > 0) aiPoints--;
                }

                Console.WriteLine($"🏁 Score: Player {playerPoints} | AI {aiPoints}");
                Console.WriteLine();
            }

            Console.WriteLine("🎉 Game over!");
            if (playerPoints > aiPoints)
                Console.WriteLine("🏆 You are the winner!");
            else if (aiPoints > playerPoints)
                Console.WriteLine("🤖 AI wins! Better luck next time.");
            else
                Console.WriteLine("🔁 It's a draw!");

            Console.WriteLine("\nDo you want to play again? (y/n): ");
            string input = Console.ReadLine()?.Trim().ToLower();
            if (input == "y")
            {
                Console.Clear();
                PlayGame();
            }
            else
            {
                Console.WriteLine("Thanks for playing! Goodbye.");
            }
        }
    }
}
