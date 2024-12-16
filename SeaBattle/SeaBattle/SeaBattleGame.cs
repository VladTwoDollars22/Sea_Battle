﻿namespace SeaBattle
{
    public struct Player
    {
        public Profile Profile;

        public int RoundWins;

        public PlayerController PlayerController;
        public Player(string NickName)
        {
            Profile = new Profile(NickName);
            RoundWins = 0;

            PlayerController = new PlayerController();
        }
        public void LoadProfileData()
        {
            Profile.Initialize();
        }
        public void SaveProfileData()
        {
            Profile.Save();
        }
        public void RoundWin()
        {
            RoundWins++;
            Profile.RoundWins++;
        }
        public void RoundLoss()
        {
            Profile.RoundLosses++;
        }
        public void Win()
        {
            Profile.Wins++;
        }
        public void Lose()
        {
            Profile.Losses++;
        }
        public void ResetPlayerController()
        {
            PlayerController.Reset();
        }
        
    }
    public class SeaBattleGame
    {
        private Player _player1 = new("Chupa");
        private Player _player2 = new("Pupc");

        private int _winsTriggerCount = 3;

        private GameMode gameMode = GameMode.EVE;
        public void StartGame()
        {
            Initialization();

            while (!HaveWinner())
            {
                RoundProcess();
            }

            EndProcess();
        }
        private bool HaveWinner()
        {
            return _player1.RoundWins >= _winsTriggerCount || _player2.RoundWins >= _winsTriggerCount;
        }
        private void RoundProcess()
        {
            SeaBattleRound round = new(gameMode,_player1.PlayerController,_player2.PlayerController);
            round.GameProcess();
            RoundResult result = round.GetRoundResult();
            CalculateRoundWinner(result);
            ResetPlayers();
        }
        private void ResetPlayers()
        {
            _player1.ResetPlayerController();
            _player2.ResetPlayerController();
        }
        private void CalculateRoundWinner(RoundResult result)
        {
            if (result == RoundResult.Player1Win)
            {
                _player1.RoundWin();
                _player2.RoundLoss();
            }       
            if (result == RoundResult.Player2Win)
            {
                _player2.RoundWin();
                _player1.RoundLoss();
            }
                
        }
        private void EndProcess()
        {
            CalculateTotalWinner();
            SaveUsersData();
            EndVisual();
        }
        private void CalculateTotalWinner()
        {
            if (_player1.RoundWins >= _winsTriggerCount)
            {
                _player1.Win();
                _player2.Lose();
            }
            else
            {
                _player1.Lose();
                _player2.Win();
            }
        }
        private void SaveUsersData()
        {
            _player1.SaveProfileData();
            _player2.SaveProfileData();
        }
        private void EndVisual()
        {
            Console.WriteLine("Кількість перемoг у гравців:");
            Console.WriteLine("Гравець 1:" + _player1.RoundWins);
            Console.WriteLine("Гравець 2:" + _player2.RoundWins);
        }
        private void Initialization()
        {
            InitializateUsers();
        }
        private void InitializateUsers()
        {
            _player1.LoadProfileData();
            _player2.LoadProfileData();
        }
    }
}
