using AiAnnotations;
using AiAnnotations.Types;

namespace WordWheelPlayer.Test;

[AiGenerated(Authorship.MostlyAi)]
public class GameLetterTests
{
    [Fact]
    public void GameLetter_ShouldInitializePropertiesCorrectly()
    {
        // Arrange & Act
        var letter = new GameLetter
        {
            Letter = "A",
            Used = true,
            MustInclude = false
        };

        // Assert
        Assert.Equal("A", letter.Letter);
        Assert.True(letter.Used);
        Assert.False(letter.MustInclude);
    }

    [Fact]
    [AiGenerated("Merged arrange and act")]
    public void GameLetter_ShouldAllowPropertyChanges()
    {
        // Arrange/Act
        var letter = new GameLetter
        {
            Letter = "B",
            Used = false,
            MustInclude = true
        };

        // Assert
        Assert.Equal("B", letter.Letter);
        Assert.False(letter.Used);
        Assert.True(letter.MustInclude);
    }

    [Fact]
    public void GameLetter_ShouldHandleNullLetter()
    {
        // Arrange
        var letter = new GameLetter { Letter = null };

        // Act & Assert
        Assert.Null(letter.Letter);
    }
}