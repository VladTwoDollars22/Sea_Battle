namespace SeaBattle
{
    public class SeaBattleGame
    {
        private User _user1 = new("Chupa","user1");
        private User _user2 = new("Pupc","user2");

        private Player _player1;
        private Player _player2;

        private int _player1Wins = 0;
        private int _player2Wins = 0;

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
            return _player1Wins >= _winsTriggerCount || _player2Wins >= _winsTriggerCount;
        }
        private void RoundProcess()
        {
            SeaBattleRound round = new(gameMode,_player1,_player2);
            round.GameProcess();
            RoundResult result = round.GetRoundResult();
            CalculateRoundWinner(result);
            ResetPlayers();
        }
        private void ResetPlayers()
        {
            _player1.Reset();
            _player2.Reset();
        }
        private void CalculateRoundWinner(RoundResult result)
        {
            if (result == RoundResult.Player1Win)
            {
                _player1Wins++;
                _user1.Wins++;
                _user2.Losses++;
            }       
            if (result == RoundResult.Player2Win)
            {
                _player2Wins++;
                _user2.Wins++;
                _user1.Losses++;
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
            if (_player1Wins >= _winsTriggerCount)
            {
                _user1.TotalWins++;
                _user2.TotalLosses++;
            }
            else
            {
                _user1.TotalLosses++;
                _user2.TotalWins++;
            }
        }
        private void SaveUsersData()
        {
            _user1.Save();
            _user2.Save();
        }
        private void EndVisual()
        {
            Console.WriteLine("Кількість перемoг у гравців:");
            Console.WriteLine("Гравець 1:" + _player1Wins);
            Console.WriteLine("Гравець 2:" + _player2Wins);
        }
        private void Initialization()
        {
            InitializateUsers();
            CreatePlayers();
        }
        private void InitializateUsers()
        {
            _user1.Initialize("user1",_user1.NickName);
            _user2.Initialize("user2", _user2.NickName);
        }
        private void CreatePlayers()
        {
            _player1 = new Player(_user1.NickName);
            _player2 = new Player(_user2.NickName);
        }
    }
}
