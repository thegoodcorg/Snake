using System;
using System.Collections.Generic;

namespace SnakeGame
{
    class Program
    {
        // Game settings
        static int width = 20;
        static int height = 20;
        static char[,] grid = new char[width, height];

        // Snake settings
        static Queue<Position> snake = new Queue<Position>();
        static Position head = new Position();
        static Position tail = new Position();

        // Food settings
        static Position food = new Position();

        // Movement settings
        static bool gameover = false;
        static char direction = ' ';
        static int score = 0;

        static void Main(string[] args)
        {
            Console.Title = "Snake Game";
            Console.CursorVisible = false;

            Initialize();
            Draw();

            while (!gameover)
            {
                Input();
                Logic();
                Draw();

                System.Threading.Thread.Sleep(100);
            }

            Console.ResetColor();
            Console.SetCursorPosition(0, height + 1);
            Console.WriteLine("Game Over! Final Score: " + score);
            Console.ReadKey();
        }

        static void Initialize()
        {
            // Initialize the grid with empty spaces
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    grid[i, j] = ' ';
                }
            }

            // Generate a random position for the food
            Random rand = new Random();
            int foodX = rand.Next(0, width);
            int foodY = rand.Next(0, height);
            food = new Position(foodX, foodY);

            // Place the food on the grid
            grid[food.x, food.y] = '*';

            // Add the initial position of the snake to the queue
            snake.Enqueue(new Position(width / 2, height / 2));
        }

        static void Input()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        direction = 'U';
                        break;
                    case ConsoleKey.DownArrow:
                        direction = 'D';
                        break;
                    case ConsoleKey.LeftArrow:
                        direction = 'L';
                        break;
                    case ConsoleKey.RightArrow:
                        direction = 'R';
                        break;
                    case ConsoleKey.Escape:
                        gameover = true;
                        break;
                }
            }
        }

        static void Logic()
        {
            // Get the current position of the head and calculate the new position
            head = snake.Peek();
            Position newHead = new Position(head.x, head.y);

            switch (direction)
            {
                case 'U':
                    newHead.y--;
                    break;
                case 'D':
                    newHead.y++;
                    break;
                case 'L':
                    newHead.x--;
                    break;
                case 'R':
                    newHead.x++;
                    break;
            }

            // Check if the new position is within the bounds of the grid
            if (newHead.x < 0 || newHead.x >= width || newHead.y < 0 || newHead.y >= height)
            {
                gameover = true;
                return;
            }

            // Check if the new position is the food
            if (newHead.x == food.x && newHead.y == food.y)
            {
                // Increase the score
                score++;

                // Generate a new position for the food
                Random rand = new Random();
                int foodX = rand.Next(0, width);
                int foodY = rand.Next(0, height);
                food = new Position(foodX, foodY);

                // Place the food on the grid
                grid[food.x, food.y] = '*';
            }
            else
            {
                // Remove the tail position from the queue and grid
                tail = snake.Dequeue();
                grid[tail.x, tail.y] = ' ';
            }

            // Check if the new position is already occupied by the snake
            foreach (Position pos in snake)
            {
                if (pos.x == newHead.x && pos.y == newHead.y)
                {
                    gameover = true;
                    return;
                }
            }

            // Add the new position to the queue and grid
            snake.Enqueue(newHead);
            grid[newHead.x, newHead.y] = 'O';
        }

        static void Draw()
        {
            Console.SetCursorPosition(0, 0);

            // Draw the top border of the grid
            Console.Write('+');
            for (int i = 0; i < width; i++)
            {
                Console.Write('-');
            }
            Console.WriteLine('+');

            // Draw the grid
            for (int i = 0; i < height; i++)
            {
                Console.Write('|');
                for (int j = 0; j < width; j++)
                {
                    Console.Write(grid[j, i]);
                }
                Console.WriteLine('|');
            }

            // Draw the bottom border of the grid
            Console.Write('+');
            for (int i = 0; i < width; i++)
            {
                Console.Write('-');
            }
            Console.WriteLine('+');

            // Draw the score
            Console.WriteLine("Score: " + score);
        }
    }

    class Position
    {
        public int x;
        public int y;

        public Position()
        {
            x = 0;
            y = 0;
        }

        public Position(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
    }
}