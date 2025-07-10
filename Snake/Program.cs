using System;
using System.Collections.Generic;

namespace SnakeGame
{
    public static class Program
    {
        public static void Main()
        {
            int width = 50;
            int height = 20;
            int score = 0;
            int tailLength = 1;
            int frameDelay = 100;
            bool gameOver = false;

            Coords snakeHead = new Coords(10, 5);
            List<Coords> snakeBody = new List<Coords>();
            Direction currentDirection = Direction.Right;

            Random rand = new Random();
            Coords apple = GenerateApple(rand, width, height, snakeBody);

            while (!gameOver)
            {
                Console.Clear();
                Console.WriteLine("Score: " + score);

                snakeHead = snakeHead.Move(currentDirection);

                if (IsCollision(snakeHead, snakeBody, width, height))
                {
                    Console.WriteLine("Game Over!");
                    break;
                }

                snakeBody.Add(new Coords(snakeHead.X, snakeHead.Y));
                if (snakeBody.Count > tailLength)
                    snakeBody.RemoveAt(0);

                if (snakeHead.Equals(apple))
                {
                    score++;
                    tailLength++;
                    apple = GenerateApple(rand, width, height, snakeBody);
                }

                DrawGrid(width, height, snakeHead, snakeBody, apple);

                WaitForInput(ref currentDirection, frameDelay);
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static bool IsCollision(Coords head, List<Coords> body, int width, int height)
        {
            if (head.X <= 0 || head.X >= width - 1 || head.Y <= 0 || head.Y >= height - 1)
                return true;

            foreach (var segment in body)
            {
                if (head.Equals(segment))
                    return true;
            }

            return false;
        }

        private static Coords GenerateApple(Random rand, int width, int height, List<Coords> snakeBody)
        {
            Coords newApple;
            do
            {
                newApple = new Coords(rand.Next(1, width - 1), rand.Next(1, height - 1));
            } while (snakeBody.Contains(newApple));
            return newApple;
        }

        private static void DrawGrid(int width, int height, Coords head, List<Coords> body, Coords apple)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Coords current = new Coords(x, y);

                    if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                        Console.Write("#");
                    else if (head.Equals(current))
                        Console.Write("O");
                    else if (body.Contains(current))
                        Console.Write("o");
                    else if (apple.Equals(current))
                        Console.Write("*");
                    else
                        Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

        private static void WaitForInput(ref Direction direction, int delay)
        {
            DateTime start = DateTime.Now;

            while ((DateTime.Now - start).TotalMilliseconds < delay)
            {
                if (!Console.KeyAvailable) continue;

                ConsoleKey key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.W when direction != Direction.Down:
                        direction = Direction.Up;
                        break;
                    case ConsoleKey.S when direction != Direction.Up:
                        direction = Direction.Down;
                        break;
                    case ConsoleKey.A when direction != Direction.Right:
                        direction = Direction.Left;
                        break;
                    case ConsoleKey.D when direction != Direction.Left:
                        direction = Direction.Right;
                        break;
                }
            }
        }
    }
}
