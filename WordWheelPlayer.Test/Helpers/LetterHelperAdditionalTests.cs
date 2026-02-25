using AiAnnotations;
using WordWheelPlayer.Helpers;

namespace WordWheelPlayer.Test.Helpers;

[AiGenerated]
public class LetterHelperAdditionalTests
{
    [Fact]
    public void TryShuffleLetters_WithAsterisk_PreservesAsterisk()
    {
        var result = LetterHelper.TryShuffleLetters("A*BC");

        Assert.Contains("*", result);
        Assert.Equal(4, result.Length);
    }

    [Fact]
    public void TryShuffleLetters_WithoutAsterisk_ShufflesLetters()
    {
        var result = LetterHelper.TryShuffleLetters("ABC");

        Assert.Equal(3, result.Length);
        Assert.DoesNotContain("*", result);
    }

    [Fact]
    public void TryShuffleLetters_MultipleLetters_ContainsAllLetters()
    {
        var input = "ABCDEFGHI";
        var result = LetterHelper.TryShuffleLetters(input);

        Assert.Equal(input.Length, result.Length);
        foreach (var c in input)
        {
            Assert.Contains(c, result);
        }
    }

    [Fact]
    public void ShuffleLetters_AsteriskAtEnd_Reshuffles()
    {
        var result = LetterHelper.ShuffleLetters("ABC*");

        Assert.Contains("*", result);
        Assert.False(result.EndsWith('*'));
    }
}
