//using System;
//using System.IO;
//using System.Text.Json;

//public class BestGame
//{
//    public int BestScore { get; set; }
//    public int BestWords { get; set; }
//    public DateTime BestDate { get; set; }
//}

//class Program
//{
//    static string filePath = "bestgame.json";

//    static void Main()
//    {
//        BestGame gameData = LoadGame();

//        Console.WriteLine($"Score: {gameData.BestScore}, Words: {gameData.BestWords}, Date: {gameData.BestDate}");

//        // Update and save
//        gameData.BestScore += 100;
//        gameData.BestWords += 5;
//        gameData.BestDate = DateTime.Now;

//        SaveGame(gameData);
//        Console.WriteLine("Game data updated and saved.");
//    }

//    static BestGame LoadGame()
//    {
//        if (!File.Exists(filePath))
//        {
//            var defaultGame = new BestGame
//            {
//                BestScore = 0,
//                BestWords = 0,
//                BestDate = DateTime.MinValue
//            };

//            SaveGame(defaultGame);
//            return defaultGame;
//        }

//        string json = File.ReadAllText(filePath);
//        return JsonSerializer.Deserialize<BestGame>(json);
//    }

//    static void SaveGame(BestGame game)
//    {
//        string json = JsonSerializer.Serialize(game, new JsonSerializerOptions { WriteIndented = true });
//        File.WriteAllText(filePath, json);
//    }
//}