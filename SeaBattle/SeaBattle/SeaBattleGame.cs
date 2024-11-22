using System.Text;
using SeaBattle;
public enum CellState
{
    Empty,
    HasShip,
    Missed,
    Hited,
}

public enum ShootState
{
    Hitting,
    Missing,
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



public class FieldRender
{
    public void DrawFieldsWithLabels(Field enemyField, Field playerField)
    {
        Console.Clear();

        string rowLabels = "  1 2 3 4 5 6 7 8 9";
        string header = "  EnemyField".PadRight(24) + "  YourField";

        Console.WriteLine(header);
        Console.WriteLine(rowLabels.PadRight(24) + rowLabels);

        for (int i = 0; i < enemyField.Height; i++)
        {
            char columnLabel = (char)('A' + i); 

            string enemyRow = GetRowWithLabels(enemyField.Map, i, columnLabel, isEnemy: true);
            string playerRow = GetRowWithLabels(playerField.Map, i, columnLabel, isEnemy: false);

            Console.WriteLine(enemyRow.PadRight(24) + playerRow);
        }
    }

    private string GetRowWithLabels(CellState[,] field, int rowIndex, char label, bool isEnemy)
    {
        StringBuilder row = new StringBuilder();
        row.Append(label + " "); 

        for (int j = 0; j < field.GetLength(1); j++)
        {
            row.Append(GetSymbole(field[rowIndex, j], isEnemy) + " ");
        }

        return row.ToString();
    }

    private char GetSymbole(CellState cell, bool isEnemy) => cell switch
    {
        CellState.HasShip when isEnemy => '.', 
        CellState.Empty => '.',
        CellState.Missed => '~',
        CellState.Hited => 'x',
        CellState.HasShip => 'S', 
        _ => ' '
    };

}

class SeaBattleGame   
{
    private Player _player1 = new Player();
    private Player _player2 = new Player();

    private ShipPlacer shipPlacer = new ShipPlacer();
    private FieldRender fieldRender = new FieldRender();
    private RandomPointGenerator pointGenerator = new RandomPointGenerator();

    private (int x, int y) shootPoint = (0, 0);
    static void Main(string[] args)
    {
        SeaBattleGame game = new SeaBattleGame();
        game.GameProcess();
    }

    private void GameProcess()
    {
        GenerationProcess();
        Draw();

        while (!IsEndGame())
        {
            InputProcess();
            Logic();
            Draw();
        }

        EndGame();
    }

    private void EndGame()
    {
        Console.WriteLine("Гру завершено!" + "Кількість палуб ,що залишилась:" + "Гравець один:" + _player1.HP +"Гравець два:" + _player2.HP);
    }

    private void Shoot(Player defender)
    {
        while (GetShootState(shootPoint, defender.field) != ShootState.Missing)
        {
            defender.HP--;

            defender.field.Map[shootPoint.x,shootPoint.y] = CellState.Hited;

            InputProcess();
        }

        if((GetShootState(shootPoint, defender.field) != ShootState.Missing))
        {
            defender.field.Map[shootPoint.x, shootPoint.y] = CellState.Missed;
        }
    }

    private void ShotLogic()
    {
        Shoot(_player2);
        Shoot(_player1);
    }
    private void Logic()
    {
        ShotLogic();
    }
    private void Draw()
    {
        fieldRender.DrawFieldsWithLabels(_player1.field, _player2.field);
    }

    private void GenerateFields()
    {
        _player1.field.GenerateField();
        _player2.field.GenerateField();
    }

    private void PlaceAllPlayersShips()
    {
        shipPlacer.PlaceShips(_player1.field,_player1.ships);
        shipPlacer.PlaceShips(_player2.field,_player2.ships);
    }

    private void GenerationProcess()
    {
        GenerateFields();
        PlaceAllPlayersShips();
    }

    private bool IsEndGame()
    {
        return _player1.HP == 0 || _player2.HP == 0; 
    }
    private void InputProcess()
    {
        string input = Console.ReadLine();

        if (input.Length != 2)
            return;

        shootPoint.y = GetIndex(input[1]);
        shootPoint.x = GetIndex(input[0]);
    }

    private int GetIndex(char inputChar)
    {
        if (inputChar >= 'A' && inputChar <= 'J')
            return inputChar - 'A';
        if (inputChar >= '1' && inputChar <= '9') 
            return inputChar - '1';
        return -1; 
    }

    private ShootState GetShootState((int x,int y) point, Field field)
    {
        if (shootPoint.x == -1 || shootPoint.y == -1)
        {
            return ShootState.Missing;
        }

        ShootState result = ShootState.Missing;
        CellState cell = field.Map[point.x, point.y];

        if (cell == CellState.Empty)
        {
            result = ShootState.Missing;
        }
        else if(cell == CellState.HasShip)
        {
            result = ShootState.Hitting;
        }

        return result;
    }
}

