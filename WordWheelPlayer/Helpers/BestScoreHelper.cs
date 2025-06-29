using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WordWheelPlayer.Helpers
{
    public static class BestScoreHelper
    {
        private const string FilePath = "BestGame.json";

        public static BestGame CheckForBestScore(int wordCount, int score)
        {
            var bestGameSoFar = LoadGame();

            if (score <= bestGameSoFar.BestScore && wordCount <= bestGameSoFar.BestWords)
            {
                return bestGameSoFar;
            }

            bestGameSoFar.BestScore = score;
            bestGameSoFar.BestWords = wordCount;
            bestGameSoFar.BestDate = DateTime.Now;

            SaveGame(bestGameSoFar);

            return bestGameSoFar;
        }

        public static BestGame LoadGame()
        {
            if (!File.Exists(FilePath))
            {
                var defaultGame = new BestGame
                {
                    BestScore = 0,
                    BestWords = 0,
                    BestDate = DateTime.Now
                };

                SaveGame(defaultGame);
                return defaultGame;
            }

            var json = File.ReadAllText(FilePath);

            return JsonSerializer.Deserialize<BestGame>(json);
        }

        public static void SaveGame(BestGame game)
        {
            var json = JsonSerializer.Serialize(game, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
    }
}
