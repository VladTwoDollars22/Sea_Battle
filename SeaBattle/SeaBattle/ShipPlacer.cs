namespace SeaBattle
{
 public class ShipPlacer
{
        private Random _random = new Random();
        private RandomPointGenerator _pointGenerator = new RandomPointGenerator();

        public void PlaceShips(Field field, List<int> ships)
        {
            foreach (var shipLength in ships)
            {
                bool placed = false;

                while (!placed)
                {
                    CellState[,] map = field.Map;
                    var mainShipPoint = _pointGenerator.GetRandomPoint(field.Width, field.Height);
                    int randomAxis = _random.Next(0, 2);

                    if (CanPlaceShip(mainShipPoint, shipLength, randomAxis, map))
                    {
                        PlaceShip(mainShipPoint, shipLength, randomAxis, map);
                        placed = true;
                    }
                }
            }
        }
        private bool CanPlaceShip((int X, int Y) mainPoint, int shipLength, int axis, CellState[,] field)
        {
            for (int i = 0; i < shipLength; i++)
            {
                var nextPoint = GetNextPoint(mainPoint, axis, i);

                if (!CanPlaceShipPart(nextPoint.X, nextPoint.Y, field))
                {
                    return false;
                }
            }

            return true;
        }
        private void PlaceShip((int X, int Y) mainPoint, int shipLength, int axis, CellState[,] field)
        {
            for (int i = 0; i < shipLength; i++)
            {
                var nextPoint = GetNextPoint(mainPoint, axis, i);

                field[nextPoint.X, nextPoint.Y] = CellState.HasShip;
            }
        }
        private (int X, int Y) GetNextPoint((int X, int Y) mainPoint, int axis, int delta)
        {
            int x;
            int y;

            if (axis == 1)
            {
                x = mainPoint.X + delta;
                y = mainPoint.Y;
            }
            else
            {
                x = mainPoint.X;
                y = mainPoint.Y + delta;
            }

            return (x, y);
        }
        private bool CanPlaceShipPart(int X, int Y, CellState[,] field)
        {
            if (X < 0 || X >= field.GetLength(0) || Y < 0 || Y >= field.GetLength(1))
                return false;

            return field[X, Y] == CellState.Empty;
        }
    }

}
