using System;
//Сделаю систему с хвостом змейки через структуру в массиве. запишу сюда что бы не забыть.
//В каком расстоянии от головы змейки в какую сторону находится следующий поворот хвоста. И так пока расстояние не станет 0, то есть хвост закончится. Чуть медленнее чем Queue<> мб, но не надо париться.
public class Program
{
    static int PlayerX=0;
    static int PlayerY=0;
    static int MaxX=4;
    static int MaxY=4;
    static int MinX = -4;
    static int MinY = -4;

    static ConsoleKey currentKey = ConsoleKey.W;
    //Рендер консоли
    public static void Render(int PlayerX, int PlayerY, int MaxX, int MaxY, int MinX, int MinY)
    {
        Console.Clear();
        for (int Y = MinY; Y <= MaxY; Y++)
        {
            for (int X = MinX; X <= MaxX; X++)
            {
                if (X == PlayerX && Y == PlayerY)
                {
                    Console.Write(" □");
                }
                //else if(X)
                else
                {
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
            Thread.Sleep(16); //1000/60 получается 16,(6). Игра расчитана на 60 фпс
        }
    }
    public static void Move()
    {
        switch (currentKey)
        {
            case ConsoleKey.W:
                PlayerY = Math.Clamp(PlayerY - 1, MinY, MaxY - 1);
                break;
            case ConsoleKey.S:
                PlayerY = Math.Clamp(PlayerY + 1, MinY, MaxY - 1);
                break;
            case ConsoleKey.A:
                PlayerX = Math.Clamp(PlayerX - 1, MinX, MaxX - 1);
                break;
            case ConsoleKey.D:
                PlayerX = Math.Clamp(PlayerX + 1, MinX, MaxX - 1);
                break;
        }
    }
    public static void Update()
    {
        Render(PlayerX, PlayerY, MaxX, MaxY, MinX, MinY);
        Move();
        Thread.Sleep(250); //"1000/60 получается 16,(6). Игра расчитана на 60 фпс" - могли бы мы так сказать, но передвижение слишком быстрое, тикрейт будет 5 так что 1000/4=250

    }
    public static void Main()
    {
        Task.Run(() => InputReader()); //Чтобы ввод всегда считывался
        while (true)
        {
            Update();
        }
    }
}
