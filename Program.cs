using static System.Console;
namespace AK8PO_RefactoredSnake
{
    class Program
    {
        const int ScreenWidth = 32;
        const int ScreenHeight = 16;
        const int GameSpeed = 500;

        static void Main(string[] args)
        {
            int score = 5;
            bool gameOver = false;
            DateTime lastMove;

            Random randomGenerator = new();
            Pixel head = new(ScreenWidth / 2, ScreenHeight / 2, ConsoleColor.Red);
            MovementType movement = MovementType.Right;

            List<Pixel> snake = [];

            SetupWindow();

            Pixel berry = new(
                randomGenerator.Next(1, ScreenWidth - 2),
                randomGenerator.Next(1, ScreenHeight - 2),
                ConsoleColor.Blue);

            while (!gameOver)
            {
                Clear();
                if (head.XPos == ScreenWidth - 1 || head.XPos == 0 || head.YPos == ScreenHeight - 1 || head.YPos == 0)
                {
                    gameOver = true;
                }

                DrawBorder();

                if (berry.XPos == head.XPos && berry.YPos == head.YPos)
                {
                    score++;
                    berry = new(randomGenerator.Next(1, ScreenWidth - 2), randomGenerator.Next(1, ScreenHeight - 2), ConsoleColor.Blue);
                }
                for (int i = 0; i < snake.Count; i++)
                {
                    DrawPixel(snake[i]);

                    if (snake[i].XPos == head.XPos && snake[i].YPos == head.YPos)
                    {
                        gameOver = true;
                    }
                }

                DrawPixel(head);

                DrawPixel(berry);

                lastMove = DateTime.Now;
                bool buttonpressed = false;
                while (true)
                {
                    if (DateTime.Now.Subtract(lastMove).TotalMilliseconds > GameSpeed) { break; }
                    if (KeyAvailable)
                    {
                        ConsoleKeyInfo toets = ReadKey(true);

                        if (toets.Key.Equals(ConsoleKey.UpArrow) && movement != MovementType.Down && !buttonpressed)
                        {
                            movement = MovementType.Up;
                            buttonpressed = true;
                        }
                        if (toets.Key.Equals(ConsoleKey.DownArrow) && movement != MovementType.Up && !buttonpressed)
                        {
                            movement = MovementType.Down;
                            buttonpressed = true;
                        }
                        if (toets.Key.Equals(ConsoleKey.LeftArrow) && movement != MovementType.Right && !buttonpressed)
                        {
                            movement = MovementType.Left;
                            buttonpressed = true;
                        }
                        if (toets.Key.Equals(ConsoleKey.RightArrow) && movement != MovementType.Left && !buttonpressed)
                        {
                            movement = MovementType.Right;
                            buttonpressed = true;
                        }
                    }
                }

                snake.Add(new Pixel(
                    head.XPos,
                    head.YPos,
                    ConsoleColor.Green
                    ));

                switch (movement)
                {
                    case MovementType.Up:
                        head.YPos--;
                        break;
                    case MovementType.Down:
                        head.YPos++;
                        break;
                    case MovementType.Left:
                        head.XPos--;
                        break;
                    case MovementType.Right:
                        head.XPos++;
                        break;
                }
                if (snake.Count > score)
                {
                    snake.RemoveAt(0);
                }
            }
            SetCursorPosition(ScreenWidth / 5, ScreenHeight / 2);
            WriteLine("Game over, Score: " + score);
            SetCursorPosition(ScreenWidth / 5, ScreenHeight / 2 + 1);
        }

        private static void DrawBorder()
        {
            ForegroundColor = ConsoleColor.Green;
            for (int i = 0; i < ScreenWidth; i++)
            {
                SetCursorPosition(i, 0);
                Write("■");

                SetCursorPosition(i, ScreenHeight - 1);
                Write("■");
            }

            for (int i = 0; i < ScreenHeight; i++)
            {
                SetCursorPosition(0, i);
                Write("■");

                SetCursorPosition(ScreenWidth - 1, i);
                Write("■");
            }
        }

        private static void DrawPixel(Pixel pixel)
        {
            SetCursorPosition(pixel.XPos, pixel.YPos);
            ForegroundColor = pixel.ForegroundColor;
            Write("■");
            SetCursorPosition(0, 0);
        }

        private static void SetupWindow()
        {
            SetWindowSize(32, 16);
            CursorVisible = false;
        }

        class Pixel(int xPos, int yPos, ConsoleColor consoleColor = ConsoleColor.Black)
        {
            public int XPos { get; set; } = xPos;
            public int YPos { get; set; } = yPos;
            public ConsoleColor ForegroundColor { get; set; } = consoleColor;
        }

        enum MovementType
        {
            None = 0,
            Left = 1,
            Right = 2,
            Up = 3,
            Down = 4,
        }
    }
}
