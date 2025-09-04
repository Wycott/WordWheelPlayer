using AiAnnotations;
using AiAnnotations.Types;
using WordWheelPlayer.Helpers;

namespace WordWheelPlayer.Test.Helpers;

[AiGenerated("Changed vars to constants", Authorship.MostlyAi)]
public class LetterHelperTests
{
    [Fact]
    public void ShuffleLetters_ShouldReturnShuffledString()
    {
        // Arrange
        const string Input = "ABC";

        // Act
        var result = LetterHelper.ShuffleLetters(Input);

        // Assert
        Assert.Equal(Input.Length, result.Length);
        Assert.True(Input.All(c => result.Contains(c)));
    }

    [Fact]
    public void ShuffleLetters_ShouldHandleAsteriskCorrectly()
    {
        // Arrange
        const string Input = "AB*C";

        // Act
        var result = LetterHelper.ShuffleLetters(Input);

        // Assert
        Assert.Equal(Input.Length, result.Length);
        Assert.Contains("*", result);
        Assert.True(result.Replace("*", "").All(c => Input.Contains(c)));
    }

    [Fact]
    public void ShuffleLetters_ShouldReturnEmptyStringForEmptyInput()
    {
        // Arrange
        const string Input = "";

        // Act
        var result = LetterHelper.ShuffleLetters(Input);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ShuffleLetters_ShouldHandleSingleCharacterInput()
    {
        // Arrange
        const string Input = "A";

        // Act
        var result = LetterHelper.ShuffleLetters(Input);

        // Assert
        Assert.Equal("A", result);
    }
}