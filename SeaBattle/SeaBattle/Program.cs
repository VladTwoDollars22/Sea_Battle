

class Program
{
    //робимо два поля,поле бота й поле гравця
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
    static List<int> ships = new List<int>{4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };

    static void Main(string[] args)
    {
        GameProcess();
    }

    static void GameProcess()
    {
        GenerationProcess();
        Draw(playerField);
        Draw(botField);
    }

    static void GenerateFields()
    {
        playerField = GenerateField();
        botField = GenerateField();
    }

    static void PlaceAllPlayersShips()
    {
        PlaceShips(playerField);
        PlaceShips(botField);
    }
    static void GenerationProcess()
    {
        GenerateFields();
        PlaceAllPlayersShips();
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
            bool placed = false;

            while (!placed)
            {
                var mainShipPoint = GetRandomPoint();
                int randomAxis = random.Next(0, 2);

                if (CanPlaceShip(mainShipPoint, ships[i], randomAxis, field))
                {
                    PlaceShip(mainShipPoint, ships[i], randomAxis, field);
                    placed = true;
                }
            }
        }
    }

    static bool CanPlaceShip((int X, int Y) mainPoint, int shipLength, int axis, char[,] field)
    {
        for (int i = 0; i < shipLength; i++)
        {
            var nextPoint = GetNextPoint(mainPoint, axis, i);

            if (!CanPlaceShipPart(nextPoint.X, nextPoint.Y, field))
            {
                return false;
            }
        }

        return true;
    }

    static void PlaceShip((int X, int Y) mainPoint, int shipLength, int axis, char[,] field)
    {
        for (int i = 0; i < shipLength; i++)
        {
            var nextPoint = GetNextPoint(mainPoint, axis, i);

            PlaceShipPart(nextPoint.X, nextPoint.Y, field);
        }
    }

    static (int X,int Y) GetNextPoint((int X, int Y) mainPoint,int axis,int delta)
    {
        int x;
        int y;

        if (axis == 1)
        {
            x = mainPoint.X + delta;
            y = mainPoint.Y;
        }
        else
        {
            x = mainPoint.X;
            y = mainPoint.Y + delta;
        }

        return (x, y);
    }

    static void PlaceShipPart(int X,int Y, char[,] field)
    {
        field[X, Y] = 'S';
    }

    static bool CanPlaceShipPart(int X, int Y, char[,] field)
    {
        if (X < 0 || X >= height || Y < 0 || Y >= width)
            return false;

        if (field[X, Y] == 'S')
            return false;

        return true;
    }

    static (int X,int Y) GetRandomPoint()
    {
        int x = random.Next(0, height);
        int y = random.Next(0, width);

        return (x, y);
    }
}

