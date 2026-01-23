using AiAnnotations;
using AiAnnotations.Types;

namespace WordWheelPlayer.Test;

[AiGenerated(Authorship.Ai)]
public class BestGameTests
{
    [Fact]
    public void WhenAchieved_BestDateAfterGameStart_ReturnsJustNow()
    {
        // Arrange
        var game = new BestGame
        {
            BestDate = new DateTime(2025, 6, 30, 15, 0, 0) // 3 PM
        };

        var gameStartTime = new DateTime(2025, 6, 30, 14, 0, 0); // 2 PM

        // Act
        var result = game.WhenAchieved(gameStartTime);

        // Assert
        Assert.Equal("Just now", result);
    }

    [Fact]
    public void WhenAchieved_BestDateBeforeGameStart_ReturnsFormattedDate()
    {
        // Arrange
        var bestDate = new DateTime(2024, 12, 25, 9, 30, 15);

        var game = new BestGame
        {
            BestDate = bestDate
        };
        
        var gameStartTime = new DateTime(2025, 1, 1);

        // Act
        var result = game.WhenAchieved(gameStartTime);

        // Assert
        var expected = bestDate.ToString("dd/MM/yyyy HH:mm:ss");
        Assert.Equal(expected, result);
    }

    [Fact]
    public void WhenAchieved_BestDateEqualsGameStart_ReturnsFormattedDate()
    {
        // Arrange
        var bestDate = new DateTime(2023, 8, 1, 12, 0, 0);

        var game = new BestGame
        {
            BestDate = bestDate
        };

        var gameStartTime = new DateTime(2023, 8, 1, 12, 0, 0);

        // Act
        var result = game.WhenAchieved(gameStartTime);

        // Assert
        var expected = bestDate.ToString("dd/MM/yyyy HH:mm:ss");
        Assert.Equal(expected, result);
    }
}