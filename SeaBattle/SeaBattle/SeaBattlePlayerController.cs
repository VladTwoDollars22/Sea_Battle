namespace SeaBattle
{
    public class SeaBattlePlayerController
    {
        public string NickName;
        public bool isBot;
        public Field field;
        public List<int> ships;

        public int HP { get; private set; }

        public (int width, int heigth) radarArea; 
        public bool usingRadar = false;
        public (int x,int y) radarPoint;
        public int radarsCount;

        public SeaBattlePlayerController(string nickName)
        {
            NickName = nickName;
            radarsCount = 1;
            radarPoint = (-1, -1);
            radarArea = (3, 3);

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
        public void Reset()
        {
            HP = ships.Sum();
        }
    }
}
