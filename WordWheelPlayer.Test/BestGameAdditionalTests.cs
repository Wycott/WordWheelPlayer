using AiAnnotations;
using WordWheelPlayer.Helpers;

namespace WordWheelPlayer.Test;

[AiGenerated]
public class BestGameAdditionalTests
{
    [Theory]
    [InlineData("")]
    [InlineData("not json")]
    public void LoadGame_DamagedFile_StartsFromNoBestScore(string fileContents)
    {
        const string FilePath = "BestGame.json";

        File.WriteAllText(FilePath, fileContents);

        try
        {
            Assert.Equal(0, BestScoreHelper.LoadGame().BestScore);
        }
        finally
        {
            File.Delete(FilePath);
        }
    }

    [Fact]
    public void BestGame_DefaultValues_AreSetCorrectly()
    {
        var game = new BestGame();

        Assert.Equal(0, game.BestScore);
        Assert.Equal(default(DateTime), game.BestDate);
    }

    [Fact]
    public void BestGame_Properties_CanBeSet()
    {
        var date = new DateTime(2024, 5, 15);
        var game = new BestGame
        {
            BestScore = 500,
            BestDate = date
        };

        Assert.Equal(500, game.BestScore);
        Assert.Equal(date, game.BestDate);
    }

    [Fact]
    public void WhenAchieved_SameTime_ReturnsFormattedDate()
    {
        var exactTime = new DateTime(2024, 3, 10, 10, 30, 0);
        var game = new BestGame { BestDate = exactTime };

        var result = game.WhenAchieved(exactTime);

        Assert.Equal(exactTime.ToString("dd/MM/yyyy HH:mm:ss"), result);
    }
}
