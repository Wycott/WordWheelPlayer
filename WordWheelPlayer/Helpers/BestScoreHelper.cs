using AiAnnotations;
using AiAnnotations.Types;
using System.Text.Json;

namespace WordWheelPlayer.Helpers;

[AiGenerated(Authorship.MostlyAi)]
public static class BestScoreHelper
{
    private const string FilePath = "BestGame.json";

    public static BestGame CheckForBestScore(int score)
    {
        var bestGameSoFar = LoadGame();

        if (score <= bestGameSoFar.BestScore)
        {
            return bestGameSoFar;
        }

        bestGameSoFar.BestScore = score;
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