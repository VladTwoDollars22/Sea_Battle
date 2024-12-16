using System.Text.Json;
using System.Text.Json.Serialization;

namespace SeaBattle
{
    public class Profile
    {
        public string NickName;
        public int RoundWins;
        public int RoundLosses;
        public int Wins;
        public int Losses;

        [JsonIgnore]
        private string filePath;

        private JsonSerializerOptions _options = new JsonSerializerOptions { WriteIndented = true, IncludeFields = true };
        public Profile() { }

        public Profile(string nickName)
        {
            NickName = nickName;
            RoundWins = 0;
            RoundLosses = 0;
            Wins = 0;
            Losses = 0;
            filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, NickName);
        }
        public void Initialize()
        {
            Profile user = LoadUser();
            LoadStatistics(user);
        }
        private Profile LoadUser()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, NickName);

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                var user = JsonSerializer.Deserialize<Profile>(json, _options);
                user.filePath = filePath;
                return user;
            }

            var newUser = new Profile(NickName);
            newUser.Save();
            return newUser;
        }
        private void LoadStatistics(Profile user)
        {
            if (user.NickName != null)
                NickName = user.NickName;
            RoundWins = user.RoundWins;
            RoundLosses = user.RoundLosses;
            Wins = user.Wins;
            Losses = user.Losses;
        }
        public void Save()
        {
            string json = JsonSerializer.Serialize(this, _options);
            File.WriteAllText(filePath, json);
        }
    }
}
