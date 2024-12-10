namespace SeaBattle
{
    public class SeaBattleGame
    {
        private int _player1Wins = 0;
        private int _player2Wins = 0;

        private int _winsTriggerCount = 3;

        private GameMode gameMode = GameMode.EVE;
        public void StartGame()
        {
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
            SeaBattleRound round = new(gameMode);
            round.GameProcess();
            RoundResult result = round.GetRoundResult();
            CalculateWinner(result);
        }
        private void CalculateWinner(RoundResult result)
        {
            if (result == RoundResult.Player1Win)
                _player1Wins++;
            if (result == RoundResult.Player2Win)
                _player2Wins++;
        }
        private void EndProcess()
        {
            EndVisual();
        }
        private void EndVisual()
        {
            Console.WriteLine("Кількість перемoг у гравців:");
            Console.WriteLine("Гравець 1:" + _player1Wins);
            Console.WriteLine("Гравець 2:" + _player2Wins);
        }

    }
}
