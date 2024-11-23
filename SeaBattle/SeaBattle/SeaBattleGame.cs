﻿using SeaBattle;
public enum CellState
{
    Empty,
    HasShip,
    Missed,
    Hited,
}

public enum ShootState
{
    Missing,
    Hitting,
    Repeating,
}
class SeaBattleGame   
{
    private Player _player1 = new Player();
    private Player _player2 = new Player();

    private ShipPlacer shipPlacer = new ShipPlacer();
    private FieldRender fieldRender = new FieldRender();
    private RandomPointGenerator pointGenerator = new RandomPointGenerator();

    private bool isPlayer1Turn = true;
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
            Logic();
        }

        EndGame();
    }

    private void EndGame()
    {
        Console.WriteLine("Гру завершено!" + "Кількість палуб ,що залишилась:" + "Гравець один:" + _player1.HP +"Гравець два:" + _player2.HP);
    }
    private void ShootLogic()
    {
        int swapsCount = 0;
        Player attacker = _player1;
        Player defender = _player2;

        while (true)
        {
            Console.WriteLine($"Хід Гравця {(attacker == _player1 ? "1" : "2")}. Введіть координати (наприклад, A1):");
            (int x, int y) = GetInput();

            if (x == -1 || y == -1)
            {
                Console.WriteLine("Некоректні дані спробуйте вести знову");
                continue;
            }

            ShootState state = GetShootState((x, y), defender.field);

            if (state == ShootState.Hitting)
            {
                Console.WriteLine("Попали!");
                defender.HP--;
                defender.field.Map[x, y] = CellState.Hited;
            }

            else if (state == ShootState.Missing)
            {
                Console.WriteLine("Промазали!");
                defender.field.Map[x, y] = CellState.Missed;

                Player temp = attacker;
                attacker = defender;
                defender = temp;

                swapsCount++;
            }
            else
            {
                Console.WriteLine("Сюди ви вже стріляли. Спробуйте знову.");
                continue;
            }

            fieldRender.DrawField(attacker.field,defender.field);
        }
    }
    private void Logic()
    {
        ShootLogic();
    }
    private void Draw()
    {
        fieldRender.DrawField(_player1.field, _player2.field);
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
    private (int,int) GetInput()
    {
        string input = Console.ReadLine();

        if (input.Length != 2)
            return (-1,-1);

        return (GetIndex(input[0]), GetIndex(input[1]));
    }

    private int GetIndex(char inputChar)
    {
        if (inputChar >= 'A' && inputChar <= 'I')
            return inputChar - 'A';
        if (inputChar >= '1' && inputChar <= '9') 
            return inputChar - '1';
        return -1; 
    }

    static ShootState GetShootState((int x, int y) point, Field field)
    {
        ShootState result;
        CellState cell = field.Map[point.x, point.y];

        if (cell == CellState.Empty)
        {
            result = ShootState.Missing;
        }
        else if (cell == CellState.HasShip)
        {
            result = ShootState.Hitting;
        }
        else
        {
            result = ShootState.Repeating;
        }

        return result;
    }
}
