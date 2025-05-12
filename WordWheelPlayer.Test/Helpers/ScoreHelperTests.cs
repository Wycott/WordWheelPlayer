using AiAnnotations;
using AiAnnotations.Types;
using WordWheelPlayer.Helpers;

namespace WordWheelPlayer.Test.Helpers;

public class ScoreHelperTests
{
    [Theory]
    [InlineData("hi", 0)]
    [InlineData("cat", 1)]
    [InlineData("dogs", 2)]
    [InlineData("apple", 3)]
    [InlineData("banana", 5)]
    [InlineData("compute", 8)]
    [InlineData("keyboard", 13)]
    [InlineData("telephone", 21)]
    [AiGenerated(Authorship.MostlyAi)]
    public void CalculateWordScore_ShouldReturnCorrectScore(string word, int expectedScore)
    {
        // Act
        var actualScore = ScoreHelper.CalculateWordScore(word);

        // Assert
        Assert.Equal(expectedScore, actualScore);
    }
}