
using System;
using System.Data.SqlTypes;

class Program
{
    //робимо два поля,боле бота й поле гравця
    //розташування корабликів
    //ігровий процес
    //завершення гри 

    static Random random = new Random();

    //field
    static int width = 20;
    static int height = 12;

    //player
    static char[,] playerField;

    //bot
    static char[,] botField;

    //ships
    static List<int> ships = new List<int>{ 4, 3, 2, 2, 2, 1, 1 };

    static void Main(string[] args)
    {
        GenerationProcess();
        Draw(playerField);
    }

    static void GenerationProcess()
    {
        char[,] field = GenerateField();
        playerField = field;
        botField = field; 


    }

    static char[,] GenerateField()
    {
        char[,] field = new char[height,width]; 

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                field[i, j] = '.';
            }
        }

        return field;
    }

    static void Draw(char[,] field)
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Console.Write(field[i, j]);
            }

            Console.WriteLine();
        }
    }

    static void PlaceShips(char[,] field)
    {
        for (int i = 0;i < ships.Count;i++)
        {
            for(int j = 0; j <= ships[i]; j++)
            {
                if(j == 0)
                {
                    var mainShipPoint = GetRandomPoint();
                }
            }
        }
    }

    static (int X,int Y) GetRandomPoint()
    {
        int x = random.Next(0, height);
        int y = random.Next(0, width);

        return (x, y);
    }
}

