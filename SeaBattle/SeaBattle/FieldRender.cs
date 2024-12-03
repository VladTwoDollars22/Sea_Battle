using System.Text;

namespace SeaBattle
{
    static class FieldRender
    {
        public static void DrawField(Player player1, Player player2,GameMode gameMode)
        {
            string rowLabels = "  1 2 3 4 5 6 7 8 9";
            string header = "  Player 1 Field".PadRight(24) + "  Player 2 Field";

            Console.Clear();
            Console.WriteLine(header);
            Console.WriteLine(rowLabels.PadRight(24) + rowLabels);

            for (int i = 0; i < player1.field.Height; i++)
            {
                char columnLabel = (char)('A' + i);

                string player1Row = GetRowWithLabels(player1, i, columnLabel,gameMode,player1.isBot);
                string player2Row = GetRowWithLabels(player2, i, columnLabel,gameMode, player2.isBot);

                Console.WriteLine(player1Row.PadRight(24) + player2Row);
            }
        }

        static string GetRowWithLabels(Player player, int rowIndex, char label,GameMode gameMode, bool isBot)
        {
            StringBuilder row = new StringBuilder();
            row.Append(label + " ");

            for (int j = 0; j < player.field.GetMapLength(); j++)
            {
                CellState cell = player.field.GetCell(rowIndex, j);

                bool isVisible = ShouldDisplayCell(gameMode,isBot);
                row.Append(GetSymbol(cell, isVisible) + " ");
            }

            return row.ToString();
        }

        static char GetSymbol(CellState cell, bool isVisible)
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

        static bool ShouldDisplayCell(GameMode gameMode,bool isBot)
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
    }
}
