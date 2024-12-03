﻿using System.Runtime.CompilerServices;
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
public enum GameMode
{
    PVP,
    PVE,
    EVE,
}

public enum Action
{
    Radar,
    None,
}
class SeaBattleGame   
{
    private Player _player1 = new Player();
    private Player _player2 = new Player();

    private (int x, int y) _actionPoint;

    private Player _attacker;
    private Player _defender;

    private int _transitionTime = 1;

    GameMode currentGameMode;
    public void GameProcess()
    {
        while (!HaveWinner())
        {
            GameLoop();
        }

        EndGame();
    }

    private void EndGame()
    {
        Player winner = GetMainWinner();

        Console.WriteLine("Остаточно переміг гравець" + winner.nickName);
        Console.WriteLine("Кількість перемог у цього гравця:" + winner.wins);
    }

    private Player GetMainWinner()
    {
        if (_player1.wins >= 3)
            return _player1;
        else
            return _player2;
    }

    private bool HaveWinner()
    {
        return _player1.wins >= 3 || _player2.wins >= 3;
    }
    public void GameLoop()
    {
        Initialization();
        Draw();

        while (!IsEndGame())
        {
            InputProcess();
            Logic();
            Draw();
        }

        EndGameLoop();
    }

    private void Initialization()
    {
        Start();
        GenerationProcess();
    }
    private void Start()
    {
        SetRoles();
        SetGameMode(GameMode.PVE);
    }
    private void SetRoles()
    {
        _attacker = _player1;
        _defender = _player2;
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
        FieldRender.DrawField(_player1,_player2,currentGameMode);
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
        if (_attacker.usingRadar)
        {
            TransitionVisual();
            return;
        }

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

            TransitionVisual();

           (_defender, _attacker) = (_attacker, _defender);
        }
    }
    private void EndGameLoop()
    {
        Console.WriteLine("Гру завершено!" + "Кількість палуб ,що залишилась:" + "Гравець один:" + _player1.HP + "Гравець два:" + _player2.HP);

        GetWinner().Win();
        Console.WriteLine("Кількість перемог" + "Гравець один:" + _player1.HP + "Гравець два" + _player2.HP);

        TransitionVisual();
    }
    private Player GetWinner()
    {
        if (_player1.HP == 0)
            return _player1;
        else
            return _player2;
    }
    private void InputProcess()
    {
        _attacker.radarPoint = (-1, -1);
        _attacker.usingRadar = false;
        if (_attacker.isBot == false)
        {
            (int x, int y, Action action) input = GetInput();

            if(input.action == Action.None)
            {
                _actionPoint = (input.x, input.y);
            }

            else if(input.action == Action.Radar && _attacker.radarsCount > 0)
            {
                _actionPoint = (-1, -1);
                _attacker.radarPoint = (input.x, input.y);
                _attacker.usingRadar = true;
                _attacker.UseRadar();
            }
        }
        else
        {
            _actionPoint = _defender.field.GetRandomPoint();
        }
    }
    private (int ,int,Action) GetInput()
    {
        string input = Console.ReadLine();
        Action action = Action.None;

        if (input.Length >= 2)
        {
            if (input.Length > 2 && (input[2] == 'R' || input[2] == 'r'))
            {
                action = Action.Radar;
            }
            else
            {
                action = Action.None;
            }

            int x = GetIndex(input[0]);
            int y = GetIndex(input[1]);

            return (x, y, action);
        }

        return (-1, -1, Action.None);
    }

    private int GetIndex(char inputChar)
    {
        if (inputChar >= 'A' && inputChar <= 'I')
            return inputChar - 'A';
        if (inputChar >= '1' && inputChar <= '9') 
            return inputChar - '1';
        return -1; 
    }

    private void TransitionVisual()
    {
        for(int i = 1;i <= 3; i++)
        {
            Console.Write(".");
            Thread.Sleep(_transitionTime);
        }
    }

    private void SetGameMode(GameMode mode)
    {
        currentGameMode = mode;

        switch (mode)
        {
            case GameMode.PVP:
                _player1.isBot = false;
                _player2.isBot = false;
                break;
            case GameMode.PVE:
                _player1.isBot = false;
                _player2.isBot = true;
                break;
            default:
                _player1.isBot = true;
                _player2.isBot = true;
                break;
        }
    }
}

