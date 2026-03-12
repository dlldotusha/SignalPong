using System;

public class Program
{
    public static void Render(int PlayerX, int PlayerY, int MaxX, int MaxY)
    {
        Console.WriteLine("Render");
        for (int Y = 0; Y < MaxY; Y++)
        {
            for (int X = 0; X < MaxX; X++)
            {
                if (X == PlayerX && Y == PlayerY)
                {
                    Console.Write(" □");
                }
                else
                {
                    Console.Write(" ■");
                }
            }
            Console.Write("\n");
        }
    }

    public static void Main()
    {
        Render(1, 1, 5, 5);
    }
}
