using System.Linq;
using System.Threading;

//Сделаю систему с хвостом змейки через структуру в массиве. запишу сюда что бы не забыть.
///В каком расстоянии от головы змейки в какую сторону находится следующий поворот хвоста. И так пока расстояние не станет 0, то есть хвост закончится. Чуть медленнее чем Queue<> мб, но не надо париться.
//Ладно, нет, сделаю сейчас через очередь. Дальше буду проблемы с маштабированием поля в мультиплеере.
public class Program
{
    static Random rand = new Random(); //зачем это писать я так и не выяснил. но все так делают значит и я буду
    static int PlayerX = 0;
    static int PlayerY = 0;
    static int MaxX = 4;
    static int MaxY = 4;
    static int MinX = -4;
    static int MinY = -4;
    //"?" для null в координатах еды - когда еды нет на карте
    static int? EatX = null;
    static int? EatY = null;
    static byte ToEat = 0;

    static ConsoleKey currentKey = ConsoleKey.W;
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

    //Рендер консоли
    public static void Render(int PlayerX, int PlayerY, int MaxX, int MaxY, int MinX, int MinY)
    {
        //Console.Clear();
        Console.SetCursorPosition(0, 0);
        for (int Y = MinY; Y <= MaxY; Y++)
        {
            for (int X = MinX; X <= MaxX; X++)
            {
                if(snake.Any(p => p.X == X && p.Y == Y))
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
                else if(X == EatX && Y == EatY)
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
    static void InputReader()
    {
        while (true)
        {
            currentKey = Console.ReadKey(true).Key;
            Thread.Sleep(1); //Чтобы не нагружать систему
        }
    }
    public static void Move()
    {
        switch (currentKey)
        {
            case ConsoleKey.W:
                PlayerY = Math.Clamp(PlayerY - 1, MinY, MaxY);
                break;
            case ConsoleKey.S:
                PlayerY = Math.Clamp(PlayerY + 1, MinY, MaxY);
                break;
            case ConsoleKey.A:
                PlayerX = Math.Clamp(PlayerX - 1, MinX, MaxX);
                break;
            case ConsoleKey.D:
                PlayerX = Math.Clamp(PlayerX + 1, MinX, MaxX);
                break;
        }
    }
    public static void SpawnEat()
    {
        if (ToEat == 5)
        {
            EatX = rand.Next(MinX, MaxX);
            EatY = rand.Next(MinY, MaxY);
            if (snake.Any(p => p.X == EatX && p.Y == EatY) || (EatX==0 && EatY==0))
            {
                EatX = null;
                EatY = null ;
                ToEat = 0;
            }
        }
        ToEat++;
    }
    public static void Eating()
    {
        if (PlayerX == EatX && PlayerY == EatY)
        {
            EatX=null;
            EatY=null;
            ToEat = 0;
        }
    }
    public static void Update()
    {
        Eating();
        SpawnEat();
        Render(PlayerX, PlayerY, MaxX, MaxY, MinX, MinY);
        Move();
        EnqueueSnake();
        Thread.Sleep(500); //"1000/60 получается 16,(6). Игра расчитана на 60 фпс" - могли бы мы так сказать, но передвижение слишком быстрое, тикрейт будет 5 так что 1000/2=500

    }
    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Task.Run(() => InputReader());
        while (true)
        {
            Update();
        }
    }
}
