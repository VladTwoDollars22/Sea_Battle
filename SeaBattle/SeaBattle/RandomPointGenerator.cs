namespace SeaBattle
{   public class RandomPointGenerator
    {
        private Random _random = new Random();
        public (int X, int Y) GetRandomPoint(int fieldHeight, int fieldWidth)
        {
            int x = _random.Next(0, fieldHeight);
            int y = _random.Next(0, fieldWidth);

            return (x, y);
        }
    }
}

