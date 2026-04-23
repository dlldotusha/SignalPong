using Microsoft.AspNetCore.SignalR.Client;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
public class Program
{
    static Random rand = new Random(); //зачем это писать я так и не выяснил. но все так делают значит и я буду
    static int PlayerX = 0;
    static int PlayerY = 0;
    static int MaxX = 32;
    static int MaxY = 16;
    static int MinX = -32;
    static int MinY = -16;
    static int SnakeLength = 1; //мб пригодится
    //"?" для null в координатах еды - когда еды нет на карте
    static int? EatX = null;
    static int? EatY = null;
    static byte ToEat = 0;
    static ConsoleKey BeforeCurrentKey = ConsoleKey.S;
    static int? returnable = null;

    static ConsoleKey CurrentKey = ConsoleKey.W;
    struct GameObject
    {
        public int X;
        public int Y;
        public GameObject(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    static Queue<GameObject> snake = new();
    static void EnqueueSnake()
    {
        snake.Enqueue(new GameObject(PlayerX, PlayerY));
    }
    static void DequeueSnake()
    {
        snake.Dequeue();
    }
    public static int ConvertScreen(int pos, int min, int max)
    {
        if (pos > max)
        {
            return min;
        }
        if (pos < min)
        {
            return max;
        }
        return pos;
    }
    //Рендер консоли
    public static void Render(int PlayerX, int PlayerY, int MaxX, int MaxY, int MinX, int MinY)
    {
        //Console.Clear();
        Console.SetCursorPosition(0, 0);
        for (int Y = MinY; Y <= MaxY; Y++)
        {
            for (int X = MinX; X <= MaxX; X++)
            {
                if (snake.Any(p => p.X == X && p.Y == Y))
                {
                    if (X == PlayerX && Y == PlayerY)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(" ■");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write(" □");
                    }
                }
                else if (X == EatX && Y == EatY)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(" ■");
                }
                //else if(X)
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(" ■");
                }
            }
            Console.Write("\n");
        }
    }
    //public static void InputReader()
    //{
    //    while (true)
    //    {
    //        CurrentKey = Console.ReadKey(true).Key;
    //        Thread.Sleep(1); //Чтобы не нагружать систему
    //    }
    //}
    public static void Move()
    {
        //Запрет на движение в противоположную сторону
        if ((CurrentKey == ConsoleKey.W && BeforeCurrentKey == ConsoleKey.S) || (CurrentKey == ConsoleKey.S && BeforeCurrentKey == ConsoleKey.W) || (CurrentKey == ConsoleKey.A && BeforeCurrentKey == ConsoleKey.D) || (CurrentKey == ConsoleKey.D && BeforeCurrentKey == ConsoleKey.A))
        {
            CurrentKey = BeforeCurrentKey;
        }
        switch (CurrentKey)
        {
            case ConsoleKey.W:
                PlayerY--; break;
            case ConsoleKey.S:
                PlayerY++; break;
            case ConsoleKey.A:
                PlayerX--; break;
            case ConsoleKey.D:
                PlayerX++; break;
        }
        PlayerX = ConvertScreen(PlayerX, MinX, MaxX);
        PlayerY = ConvertScreen(PlayerY, MinY, MaxY);
        BeforeCurrentKey = CurrentKey;
    }
    public static void SpawnEat()
    {
        if (ToEat == snake.Count())
        {
            EatX = rand.Next(MinX, MaxX);
            EatY = rand.Next(MinY, MaxY);
            if (snake.Any(p => p.X == EatX && p.Y == EatY) || (EatX == 0 && EatY == 0))
            {
                EatX = null;
                EatY = null;
                ToEat = 0;
            }
        }
        ToEat++;
    }
    public static void Eating()
    {
        if (PlayerX == EatX && PlayerY == EatY)
        {
            EnqueueSnake();
            EatX = null;
            EatY = null;
            ToEat = 0;
        }
    }
    static void SnakeCollision()
    {
        if (snake.Any(p => p.X == PlayerX && p.Y == PlayerY))
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Game Over!");
            Environment.Exit(0);
        }
    }
    public static void Update()
    {
        EnqueueSnake();
        DequeueSnake();
        Eating();
        SpawnEat();
        Render(PlayerX, PlayerY, MaxX, MaxY, MinX, MinY);
        Move();
        SnakeCollision();
        Thread.Sleep(100); //тикрейт 10

    }
    public async static Task SendInputToServer()
    {
        while (true)
        {
            BeforeCurrentKey = CurrentKey;
            CurrentKey = Console.ReadKey(true).Key;
            if (CurrentKey != BeforeCurrentKey)
            {
                switch (CurrentKey)
                {
                    case ConsoleKey.W: returnable = 0; break;
                    case ConsoleKey.A: returnable = 1; break;
                    case ConsoleKey.S: returnable = 2; break;
                    case ConsoleKey.D: returnable = 3; break;
                }
                await ServerSender.SendAsync("SendInput", returnable);
            }
        }
    }
    public static void Main(string[] args)
    {
        ServerSender.StartAsync().Wait();
        //Task.Run(() => InputReader());
        Task.Run(() => SendInputToServer());
        EnqueueSnake();
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.CursorVisible = false;
        Console.Clear();

        while (true)
        {
            Update();
        }
    }
}