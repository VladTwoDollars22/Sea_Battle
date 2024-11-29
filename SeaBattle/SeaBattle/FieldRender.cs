using System.Text;

namespace SeaBattle
{
    static class FieldRender
    {
        public static void DrawField(Field attackerField, Field defenderField)
        {
            string rowLabels = "  1 2 3 4 5 6 7 8 9";
            string header = "  attacker's Field".PadRight(24) + "  defender's Field";

            Console.Clear();
            Console.WriteLine(header);
            Console.WriteLine(rowLabels.PadRight(24) + rowLabels);

            for (int i = 0; i < attackerField.Height; i++)
            {
                char columnLabel = (char)('A' + i);

                string attackerRow = GetRowWithLabels(attackerField, i, columnLabel, isDefender: false);

                string defenderRow = GetRowWithLabels(defenderField, i, columnLabel, isDefender: true);

                Console.WriteLine(attackerRow.PadRight(24) + defenderRow);
            }
        }

        static string GetRowWithLabels(Field field, int rowIndex, char label, bool isDefender)
        {
            StringBuilder row = new StringBuilder();
            row.Append(label + " ");

            for (int j = 0; j < field.GetMapLength(); j++)
            {
                CellState cell = field.GetCell(rowIndex, j);
                row.Append(GetSymbole(cell, isDefender) + " ");  
            }

            return row.ToString();
        }
        static char GetSymbole(CellState cell, bool isDefender)
        {
            return cell switch
            {
                CellState.HasShip when isDefender => '.',
                CellState.HasShip => 'S',  
                CellState.Empty => '.',  
                CellState.Missed => '~', 
                CellState.Hited => 'x',  
                _ => ' '  
            };
        }
    }
}
