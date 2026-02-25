using AiAnnotations;

namespace WordWheelPlayer.Test;

[AiGenerated]
public class GameLetterAdditionalTests
{
    [Fact]
    public void GameLetter_DefaultValues_AreCorrect()
    {
        var letter = new GameLetter();

        Assert.Null(letter.Letter);
        Assert.False(letter.Used);
        Assert.False(letter.MustInclude);
    }

    [Fact]
    public void GameLetter_EmptyString_CanBeSet()
    {
        var letter = new GameLetter { Letter = "" };

        Assert.Equal("", letter.Letter);
    }

    [Fact]
    public void GameLetter_AllPropertiesTrue_WorksCorrectly()
    {
        var letter = new GameLetter
        {
            Letter = "Z",
            Used = true,
            MustInclude = true
        };

        Assert.Equal("Z", letter.Letter);
        Assert.True(letter.Used);
        Assert.True(letter.MustInclude);
    }

    [Fact]
    public void GameLetter_CanToggleUsed()
    {
        var letter = new GameLetter { Used = false };
        letter.Used = true;

        Assert.True(letter.Used);

        letter.Used = false;
        Assert.False(letter.Used);
    }
}
