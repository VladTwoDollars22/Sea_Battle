using System.Text.Json;
using System.Text.Json.Serialization;

namespace SeaBattle
{
    public class User
    {
        public string NickName { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int TotalWins { get; set; }
        public int TotalLosses { get; set; }

        [JsonIgnore]
        private string filePath;
        public User() { }

        public User(string nickName, string fileName)
        {
            NickName = nickName;
            Wins = 0;
            Losses = 0;
            TotalWins = 0;
            TotalLosses = 0;
            filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        }
        public void Initialize(string fileName, string nickName)
        {
            User user = LoadUser(fileName, nickName);
            LoadStatistics(user);
        }
        private User LoadUser(string fileName, string nickName)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            Console.WriteLine(filePath);
            Thread.Sleep(3000);

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                var user = JsonSerializer.Deserialize<User>(json);
                user.filePath = filePath;
                return user;
            }

            var newUser = new User(nickName, fileName);
            newUser.Save();
            return newUser;
        }
        private void LoadStatistics(User user)
        {
            if (user.NickName != null)
                NickName = user.NickName;
            Wins = user.Wins;
            Losses = user.Losses;
            TotalWins = user.TotalWins;
            TotalLosses = user.TotalLosses;
        }
        public void Save()
        {
            string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
    }
}
