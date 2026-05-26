using System.Text.Json;

namespace WordWheelPlayer;

public class GameConfig
{
    private const string ConfigFilePath = "gameconfig.json";

    public int WordDisplayColumns { get; set; } = 3;

    public static GameConfig Load()
    {
        if (!File.Exists(ConfigFilePath))
        {
            return new GameConfig();
        }

        try
        {
            var json = File.ReadAllText(ConfigFilePath);
            return JsonSerializer.Deserialize<GameConfig>(json) ?? new GameConfig();
        }
        catch
        {
            return new GameConfig();
        }
    }

    public static void Save(GameConfig config)
    {
        var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(ConfigFilePath, json);
    }
}
