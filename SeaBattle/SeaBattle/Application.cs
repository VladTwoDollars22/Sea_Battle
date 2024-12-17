namespace SeaBattle
{
    internal class Application
    {
        static SeaBattleGame game;

        static string profile1;
        static string profile2;

        static GameMode gameMode = GameMode.EVE;

        static string input;

        static string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Profiles");
        static void Main(string[] args)
        {
            Initialization();
            StartGame();
        }

        static void StartGame()
        {
            game.StartGame();
        }
        static bool ProfilesReceived()
        {
            return !(string.IsNullOrEmpty(profile1) || string.IsNullOrEmpty(profile2));
        }
        static void Initialization()
        {
            ChooseProfiles();
        }
        static void ChooseProfiles()
        {
            while (!ProfilesReceived())
            {
                Draw();
                InputProcess();
                SetProfile();
            }

            EndProcess();
        }
        static void SetProfile()
        {
            if( string.IsNullOrEmpty(profile1))
            {
                profile1 = input;
                Console.WriteLine("Перший профіль обрано!");
                Thread.Sleep(2000);
            }
            else
            {
                if (profile1 == input)
                {
                    Console.WriteLine("Цей профіль уже був обраний");
                    Thread.Sleep(2000);
                    return;
                }
                    

                profile2 = input;
            }
        }
        static void InputProcess()
        {
            input = Console.ReadLine();
        }
        static void Draw()
        {
            Console.Clear();
            Console.WriteLine("Оберіть профіль з перелічених ввівши його назву,якщо хочете створити новий напишіть іншу назву якої немає в перелічених");

            if (Directory.Exists(folderPath))
            {
                string[] files = Directory.GetFiles(folderPath);

                foreach (string file in files)
                {
                    Console.WriteLine(Path.GetFileNameWithoutExtension(file));
                }
            }
            else
            {
                Console.WriteLine("У вас ще не було створених профілів,тому скоріш створіть нові!");
            }
        }

        static void EndProcess()
        {
            game = new SeaBattleGame(profile1, profile2, gameMode);

            EndVisual();
        }

        static void EndVisual()
        {
            Console.WriteLine("Профілі обрано,починаємо гру...");

            Thread.Sleep(2500);
        }
    }
}
