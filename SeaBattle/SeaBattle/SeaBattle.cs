
public enum CellState
{
    Empty,
    HasShip,
    Missed,
    Hited
}

public enum ShootResult
{
    Hitting,
    Missing
}

public class Field
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public CellState[,] Map;

    public Field(int width, int height)
    {
        Width = width;
        Height = height;
        Map = new CellState[height, width];
    }

    public CellState[,] GenerateField()
    {
        Map = new CellState[Height, Width];

        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                Map[i, j] = CellState.Empty;
            }
        }

        return Map;
    }
}

public class RandomPointGenerator()
{
    private Random _random = new Random();
    public (int X, int Y) GetRandomPoint(int fieldHeight, int fieldWidth)
    {
        int x = _random.Next(0, fieldHeight);
        int y = _random.Next(0, fieldWidth);

        return (x, y);
    }
}

public class ShipPlacer
{
    private Random _random = new Random();
    private RandomPointGenerator _pointGenerator = new RandomPointGenerator();

    public void PlaceShips(Field field, List<int> ships)
    {
        foreach (var shipLength in ships)
        {
            bool placed = false;

            while (!placed)
            {
                CellState[,] map = field.Map;
                var mainShipPoint = _pointGenerator.GetRandomPoint(field.Width, field.Height);
                int randomAxis = _random.Next(0, 2);

                if (CanPlaceShip(mainShipPoint, shipLength, randomAxis, map))
                {
                    PlaceShip(mainShipPoint, shipLength, randomAxis, map);
                    placed = true;
                }
            }
        }
    }
    private bool CanPlaceShip((int X, int Y) mainPoint, int shipLength, int axis, CellState[,] field)
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
    private void PlaceShip((int X, int Y) mainPoint, int shipLength, int axis, CellState[,] field)
    {
        for (int i = 0; i < shipLength; i++)
        {
            var nextPoint = GetNextPoint(mainPoint, axis, i);

            field[nextPoint.X, nextPoint.Y] = CellState.HasShip;
        }
    }
    private (int X, int Y) GetNextPoint((int X, int Y) mainPoint, int axis, int delta)
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
    private bool CanPlaceShipPart(int X, int Y, CellState[,] field)
    {
        if (X < 0 || X >= field.GetLength(0) || Y < 0 || Y >= field.GetLength(1))
            return false;

        return field[X, Y] == CellState.Empty;
    }
}

public class FieldRender
{
    public void Draw(Field field)
    {
        for (int i = 0; i < field.Height; i++)
        {
            for (int j = 0; j < field.Width; j++)
            {
                Console.Write(GetSymbole(field.Map[i, j]));
            }

            Console.WriteLine();
        }
    }

    private char GetSymbole(CellState cell) => cell switch
    {
        CellState.Empty => '.',
        CellState.HasShip => 'S',
        CellState.Missed => '~',
        CellState.Hited => 'x'
    };
}

class SeaBattle
{
    static int width = 20;
    static int height = 12;

    static Field playerField = new Field(width,height);

    static Field botField = new Field(width, height);

    static List<int> ships = new List<int>{4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };

    static ShipPlacer shipPlacer = new ShipPlacer();
    static FieldRender fieldRender = new FieldRender();

    static (int x, int y) shootPoint = (0, 0);



    static void Main(string[] args)
    {
        GameProcess();
    }

    static void GameProcess()
    {
        GenerationProcess();
        fieldRender.Draw(playerField);
        fieldRender.Draw(botField);
    }


    static void GenerateFields()
    {
        playerField.GenerateField();
        botField.GenerateField();
    }

    static void PlaceAllPlayersShips()
    {
        shipPlacer.PlaceShips(playerField,ships);
        shipPlacer.PlaceShips(botField, ships);
    }
    static void GenerationProcess()
    {
        GenerateFields();
        PlaceAllPlayersShips();
    }

    static void GetInput()
    {
        string input = Console.ReadLine();

        if (input.Length != 2)
            return;

        shootPoint.x = GetColumnIndex(input[0]);
        shootPoint.y = GetRowIndex(input[1]);

        if(shootPoint.x == -1 || shootPoint.y == -1)
        {
            return;
        }
    }

    static int GetColumnIndex(char columnChar)
    {
        return columnChar switch
        {
            'A' => 0,
            'B' => 1,
            'C' => 2,
            'D' => 3,
            'E' => 4,
            'F' => 5,
            'G' => 6,
            _ => -1
        };
    }

    static int GetRowIndex(char rowChar)
    {
        return rowChar switch
        {
            '1' => 0,
            '2' => 1,
            '3' => 2,
            '4' => 3,
            '5' => 4,
            '6' => 5,
            '7' => 6,
            _ => -1
        };
    }

    static ShootResult Shoot((int x,int y) point, CellState[,] cells)
    {
        ShootResult result = ShootResult.Missing;
        CellState cell = cells[point.x, point.y];

        if (cell == CellState.Empty)
        {
            cells[point.x, point.y] = CellState.Missed;
            result = ShootResult.Missing;
        }
        else if(cell == CellState.HasShip)
        {
            cells[point.x, point.y] = CellState.Hited;
            result = ShootResult.Hitting;
        }

        return result;
    }
}

