namespace SeaBattle
{
    public class Player
    {
        public string nickName;
        public bool isBot = false;
        public Field field;
        public List<int> ships;
        public int wins { get; private set; } = 0;
        public int HP { get; private set; }

        public (int width, int heigth) radarArea = (3, 3); 
        public bool usingRadar = false;
        public (int x,int y) radarPoint = (-1, -1);
        public int radarsCount = 1;

        public Player()
        {
            field = new Field(9, 9);

            ships = new List<int> {4,3,3,2,2,2,1,1,1,1};

            HP = ships.Sum();
        }

        public void TakeDamage(int damage)
        {
            HP -= damage;
        }

        public void UseRadar()
        {
            radarsCount--;
        }

        public void Win()
        {
            wins++;
        }

        public void Regenerate()
        {
            HP = ships.Sum();
        }
    }
}
