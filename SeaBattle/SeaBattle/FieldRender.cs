using System.Text;

namespace SeaBattle
{
    static class FieldRender
    {
        public static void DrawField(Field attackerField, Field defenderField,(int x,int y) visibleAreaPoint,(int width,int heigth) visibleArea)
        {
            string rowLabels = "  1 2 3 4 5 6 7 8 9";
            string header = "  attacker's Field".PadRight(24) + "  defender's Field";

            Console.Clear();
            Console.WriteLine(header);
            Console.WriteLine(rowLabels.PadRight(24) + rowLabels);

            for (int i = 0; i < attackerField.Height; i++)
            {
                char columnLabel = (char)('A' + i);

                string attackerRow = GetRowWithLabels(attackerField, i, columnLabel, isDefender: false, visibleAreaPoint, visibleArea);

                string defenderRow = GetRowWithLabels(defenderField, i, columnLabel, isDefender: true, visibleAreaPoint, visibleArea);

                Console.WriteLine(attackerRow.PadRight(24) + defenderRow);
            }
        }

        static string GetRowWithLabels(Field field, int rowIndex, char label, bool isDefender, (int x, int y) visibleAreaPoint, (int width, int heigth) visibleArea)
        {
            StringBuilder row = new StringBuilder();
            row.Append(label + " ");

            for (int j = 0; j < field.GetMapLength(); j++)
            {
                CellState cell = field.GetCell(rowIndex, j);
                bool isCellVisible = IsDisplayedCell(rowIndex,j , isDefender, visibleAreaPoint, visibleArea);
                row.Append(GetSymbole(cell, isCellVisible) + " ");  
            }

            return row.ToString();
        }
        static char GetSymbole(CellState cell, bool isVisible)
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

        static bool IsDisplayedCell(int x, int y, bool isDefender, (int x, int y) visibleAreaPoint, (int width, int height) visibleArea)
        {
            if (!isDefender) 
            {
                return true; 
            }

            if (visibleAreaPoint.x < 0 || visibleAreaPoint.y < 0)
                return false;

            if (visibleAreaPoint.x < 0 || visibleAreaPoint.y < 0)
            {
                return true;
            }

            bool isWithinVisibleArea =
                x >= visibleAreaPoint.x &&
                x < visibleAreaPoint.x + visibleArea.width &&
                y >= visibleAreaPoint.y &&
                y < visibleAreaPoint.y + visibleArea.height;

            return isWithinVisibleArea;
        }
    }
}
