using System.Text.Json;
using AiAnnotations;
using AiAnnotations.Types;

namespace WordWheelPlayer.Helpers;

[AiGenerated(Authorship.MostlyAi)]
public static class BestScoreHelper
{
    private const string FilePath = "BestGame.json";

    public static BestGame UpdateBestScoreIfHigher(int score, int wordsFound = 0, string letters = "", string centralLetter = "")
    {
        var bestGameSoFar = LoadGame();

        if (score <= bestGameSoFar.BestScore)
        {
            return bestGameSoFar;
        }

        bestGameSoFar.BestScore = score;
        bestGameSoFar.BestDate = DateTime.Now;
        bestGameSoFar.WordsFound = wordsFound;
        bestGameSoFar.Letters = letters;
        bestGameSoFar.CentralLetter = centralLetter;

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

        try
        {
            var json = File.ReadAllText(FilePath);
            var retVal = JsonSerializer.Deserialize<BestGame>(json);

            return retVal ?? new BestGame { BestScore = 0, BestDate = DateTime.Now };
        }
        catch (JsonException)
        {
            // An empty or damaged file shouldn't stop the game, so start again from no best score
            return new BestGame { BestScore = 0, BestDate = DateTime.Now };
        }
    }

    public static void SaveGame(BestGame game)
    {
        var json = JsonSerializer.Serialize(game, new JsonSerializerOptions { WriteIndented = true });

        File.WriteAllText(FilePath, json);
    }
}
