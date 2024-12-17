using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public class FieldRender
    {
        static SeaBattlePlayerController _player1;
        static SeaBattlePlayerController _player2;

        static GameMode _gameMode; 

        public void SetInfo(SeaBattlePlayerController player1, SeaBattlePlayerController player2, GameMode gameMode)
        {
            _player1 = player1;
            _player2 = player2;

            _gameMode = gameMode;
        }
        public void DrawField()
        {
            string rowLabels = "  1 2 3 4 5 6 7 8 9";
            string header = "  " + _player1.NickName + " Field".PadRight(24) + "  " + _player2.NickName + " Field";

            Console.Clear();
            Console.WriteLine(header);
            Console.WriteLine(rowLabels.PadRight(24) + rowLabels);

            for (int i = 0; i < _player1.field.Height; i++)
            {
                char columnLabel = (char)('A' + i);

                string player1Row = GetRowWithLabels(_player1,_player2, i, columnLabel,ShouldDisplayCell(_gameMode, _player1.isBot));
                string player2Row = GetRowWithLabels(_player2,_player1, i, columnLabel, ShouldDisplayCell(_gameMode, _player2.isBot));

                Console.WriteLine(player1Row.PadRight(24) + player2Row);
            }
        }

        private string GetRowWithLabels(SeaBattlePlayerController deffender,SeaBattlePlayerController attacker, int rowIndex, char label,bool isVisible)
        {
            StringBuilder row = new StringBuilder();
            row.Append(label + " ");

            for (int j = 0; j < deffender.field.GetMapLength(); j++)
            {
                bool IsVisible = isVisible;

                if (CellInArea(attacker, rowIndex, j))
                {
                    IsVisible = true;
                }

                CellState cell = deffender.field.GetCell(rowIndex, j);

                row.Append(GetSymbol(cell, IsVisible) + " ");
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

        private bool ShouldDisplayCell(GameMode gameMode,bool isBot)
        {
            if(gameMode == GameMode.PVP)
            {
                return false;
            }
            else if(gameMode == GameMode.PVE && isBot == true)
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
