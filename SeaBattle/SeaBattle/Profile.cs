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
        private string _filePath;

        private JsonSerializerOptions _options;

        private string _profilesDirectoryPath;

        public Profile() { }

        public Profile(string nickName)
        {
            NickName = nickName;
            RoundWins = 0;
            RoundLosses = 0;
            Wins = 0;
            Losses = 0;

            _options = new JsonSerializerOptions { WriteIndented = true, IncludeFields = true };

            _profilesDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Profiles");
            _filePath = Path.Combine(_profilesDirectoryPath, $"{NickName}.json");
        }
        public void Initialize()
        {
            EnsureProfilesDirectory();
            Profile user = LoadUser();
            LoadStatistics(user);
        }

        private Profile LoadUser()
        {
            _filePath = Path.Combine(_profilesDirectoryPath, $"{NickName}.json");

            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                var user = JsonSerializer.Deserialize<Profile>(json, _options);
                user._filePath = _filePath;
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
            EnsureProfilesDirectory();
            string json = JsonSerializer.Serialize(this, _options);
            File.WriteAllText(_filePath, json);
        }

        private void EnsureProfilesDirectory()
        {
            if (!Directory.Exists(_profilesDirectoryPath))
            {
                Directory.CreateDirectory(_profilesDirectoryPath);
            }
        }
    }
}
