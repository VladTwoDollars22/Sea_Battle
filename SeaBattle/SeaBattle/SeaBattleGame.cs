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
    Missing,
    Hitting,
    Repeating,
}
class SeaBattleGame   
{
    private Player _player1 = new Player();
    private Player _player2 = new Player();

    private (int x, int y) _actionPoint;

    private Player _attacker;
    private Player _defender;

    private int _transitionTime = 250;

    public SeaBattleGame()
    {
        _attacker = _player1;
        _defender = _player2;
        _player2.isBot = true;
    }
    public void GameProcess()
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
    private void GenerationProcess()
    {
        GenerateFields();
    }
    private void GenerateFields()
    {
        _player1.field.GenerateField(_player1.ships);
        _player2.field.GenerateField(_player2.ships);
    }
    private void Draw()
    {
        FieldRender.DrawField(_attacker.field, _defender.field);
    }
    private bool IsEndGame()
    {
        return _player1.HP == 0 || _player2.HP == 0;
    }
    private void Logic()
    {
        ShootLogic();
    }
    private void ShootLogic()
    {
        if (_actionPoint.x == -1 || _actionPoint.y == -1)
            return;

        Field defenderField = _defender.field;
        ShootState shootState = defenderField.GetShootState(_actionPoint);

        CellState newState;

        if (shootState == ShootState.Hitting)
        {
            newState = CellState.Hited;
            defenderField.EditCell(_actionPoint, newState);

            _defender.TakeDamage(1);
        }
        else if (shootState == ShootState.Missing)
        {
            newState = CellState.Missed;
            defenderField.EditCell(_actionPoint, newState);

            TurnTransition();

           (_defender, _attacker) = (_attacker, _defender);
        }
    }
    private void EndGame()
    {
        Console.WriteLine("Гру завершено!" + "Кількість палуб ,що залишилась:" + "Гравець один:" + _player1.HP + "Гравець два:" + _player2.HP);
    }
    private void InputProcess()
    {
        if(_attacker.isBot == false)
        {
            _actionPoint = GetInput();
        }
        else
        {
            _actionPoint = _defender.field.GetRandomPoint();
        }
    }

    private (int,int) GetInput()
    {
        string input = Console.ReadLine();

        if (input.Length != 2)
            return (-1, -1);

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

    private void TurnTransition()
    {
        for(int i = 1;i <= 3; i++)
        {
            Console.Write(".");
            Thread.Sleep(_transitionTime);
        }
    }
}

