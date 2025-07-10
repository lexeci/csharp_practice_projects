using System;

namespace NumberGuessingGame
{
    public static class Program
    {
        private const int MaxHealth = 100;
        private const int PlayerAttackDamage = 5;
        private const int EnemyAttackDamage = 7;
        private const int HealAmount = 5;

        public static void Main(string[] args)
        {
            Console.Title = "⚔️ Simple Battle Game";
            do
            {
                PlayGame();
                Console.WriteLine("\nDo you want to play again? (y/n): ");
            } while (Console.ReadLine()?.Trim().ToLower() == "y");

            Console.WriteLine("Thanks for playing! Goodbye.");
        }

        public static void PlayGame()
        {
            int playerHealth = MaxHealth;
            int enemyHealth = MaxHealth;

            Random random = new Random();

            while (playerHealth > 0 && enemyHealth > 0)
            {
                Console.Clear();
                Console.WriteLine($"⚔️ Battle Status ⚔️");
                Console.WriteLine($"Player Health: {playerHealth}HP");
                Console.WriteLine($"Enemy Health: {enemyHealth}HP\n");

                Console.WriteLine("Your turn! Choose an action:");
                Console.WriteLine("a - Attack");
                Console.WriteLine("h - Heal");
                Console.Write("Enter your choice: ");

                string? choice = Console.ReadLine()?.Trim().ToLower();

                if (choice == "a")
                {
                    enemyHealth -= PlayerAttackDamage;
                    enemyHealth = Math.Max(enemyHealth, 0); 
                    Console.WriteLine($"\nYou attacked the enemy for {PlayerAttackDamage} damage!");
                }
                else if (choice == "h")
                {
                    if (playerHealth == MaxHealth)
                    {
                        Console.WriteLine("\nYour health is already full. You wasted your turn!");
                    }
                    else
                    {
                        playerHealth += HealAmount;
                        playerHealth = Math.Min(playerHealth, MaxHealth);
                        Console.WriteLine($"\nYou healed yourself for {HealAmount} HP!");
                    }
                }
                else
                {
                    Console.WriteLine("\nInvalid input! You missed your turn.");
                }

                if (enemyHealth <= 0)
                {
                    Console.WriteLine("\n🎉 You defeated the enemy! You win!");
                    break;
                }

                Console.WriteLine("\nEnemy's turn...");
                int enemyChoice = random.Next(0, 2);

                if (enemyChoice == 0)
                {
                    playerHealth -= EnemyAttackDamage;
                    playerHealth = Math.Max(playerHealth, 0);
                    Console.WriteLine($"Enemy attacked you for {EnemyAttackDamage} damage!");
                }
                else
                {
                    if (enemyHealth == MaxHealth)
                    {
                        Console.WriteLine("Enemy tried to heal but is already at full health.");
                    }
                    else
                    {
                        enemyHealth += HealAmount;
                        enemyHealth = Math.Min(enemyHealth, MaxHealth);
                        Console.WriteLine($"Enemy healed itself for {HealAmount} HP!");
                    }
                }

                if (playerHealth <= 0)
                {
                    Console.WriteLine("\n💀 You have been defeated by the enemy...");
                    break;
                }

                Console.WriteLine("\nPress any key to continue to the next round...");
                Console.ReadKey(true);
            }
        }
    }
}
