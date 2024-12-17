using System.Text;

namespace SeaBattle
{
    public class FieldRender
    {
        static SeaBattlePlayerController _player1;
        static SeaBattlePlayerController _player2;
        static GameMode _gameMode;

        private int _fieldWidth;
        private int _fieldHeight;

        public void SetInfo(SeaBattlePlayerController player1, SeaBattlePlayerController player2, GameMode gameMode)
        {
            _player1 = player1;
            _player2 = player2;

            _gameMode = gameMode;
            _fieldWidth = player1.fieldSize.width;
            _fieldHeight = player1.fieldSize.height;
        }

        public void DrawField()
        {
            string rowLabels = "  " + string.Join(" ", Enumerable.Range(1, _fieldWidth));
            string header = "  " + _player1.NickName.PadRight(24) + "  " + _player2.NickName;

            Console.Clear();
            Console.WriteLine(header);
            Console.WriteLine(rowLabels.PadRight(24) + rowLabels);

            for (int i = 0; i < _fieldHeight; i++)
            {
                char columnLabel = (char)('A' + i);

                string player1Row = GetRowWithLabels(_player1, _player2, i, columnLabel, ShouldDisplayCell(_gameMode, _player1.isBot));
                string player2Row = GetRowWithLabels(_player2, _player1, i, columnLabel, ShouldDisplayCell(_gameMode, _player2.isBot));

                Console.WriteLine(player1Row.PadRight(24) + player2Row);
            }
        }

        private string GetRowWithLabels(SeaBattlePlayerController defender, SeaBattlePlayerController attacker, int rowIndex, char label, bool isVisible)
        {
            StringBuilder row = new StringBuilder();
            row.Append(label + " ");

            for (int j = 0; j < _fieldWidth; j++)
            {
                bool isCellVisible = isVisible;

                if (CellInArea(attacker, rowIndex, j))
                {
                    isCellVisible = true;
                }

                CellState cell = defender.field.GetCell(rowIndex, j);

                row.Append(GetSymbol(cell, isCellVisible) + " ");
            }

            return row.ToString();
        }

        private char GetSymbol(CellState cell, bool isVisible)
        {
            return cell switch
            {
                CellState.HasShip when !isVisible => '.',
                CellState.HasShip => 'S',
                CellState.Empty => '.',
                CellState.Missed => '~',
                CellState.Hited => 'x',
                _ => ' '
            };
        }

        private bool ShouldDisplayCell(GameMode gameMode, bool isBot)
        {
            if (gameMode == GameMode.PVP)
            {
                return false;
            }
            else if (gameMode == GameMode.PVE && isBot)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool CellInArea(SeaBattlePlayerController player, int pointX, int pointY)
        {
            (int x, int y) = player.radarPoint;
            (int width, int height) = player.radarArea;

            if (x == -1 || y == -1)
            {
                return false;
            }

            int left = x;
            int right = x + width - 1;
            int top = y;
            int bottom = y + height - 1;

            return pointX >= left && pointX <= right && pointY >= top && pointY <= bottom;
        }
    }
}
