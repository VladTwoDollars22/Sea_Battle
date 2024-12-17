namespace SeaBattle
{
    internal class Application
    {
        static void Main(string[] args)
        {
            SeaBattleGame game = new SeaBattleGame(new Player("Chupa"),new Player("Pupc"),GameMode.EVE);
            game.StartGame();
        }
    }
}
