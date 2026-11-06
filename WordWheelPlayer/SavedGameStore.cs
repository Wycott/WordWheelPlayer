using System.Security.Cryptography;
using System.Text.Json;

namespace WordWheelPlayer;

public class SavedGameStore(string filePath = "SavedGame.json")
{
    private const int CodeLength = 6;

    // Leaves out characters that are easily mistaken for each other (0/O and 1/I)
    private const string CodeCharacters = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

    public SavedGame? Load()
    {
        if (!File.Exists(filePath))
        {
            return null;
        }

        try
        {
            var savedGame = JsonSerializer.Deserialize<SavedGame>(File.ReadAllText(filePath));

            if (string.IsNullOrEmpty(savedGame?.Code) || string.IsNullOrEmpty(savedGame.Answer))
            {
                return null;
            }

            return savedGame;
        }
        catch (JsonException)
        {
            return null;
        }
    }

    public void Save(SavedGame savedGame)
    {
        var json = JsonSerializer.Serialize(savedGame, new JsonSerializerOptions { WriteIndented = true });

        File.WriteAllText(filePath, json);
    }

    public void Delete()
    {
        File.Delete(filePath);
    }

    public static string GenerateCode()
    {
        return RandomNumberGenerator.GetString(CodeCharacters, CodeLength);
    }
}
