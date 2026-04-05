using AiAnnotations;
using WordWheelPlayer.Helpers;

namespace WordWheelPlayer.Test.Helpers;

[AiGenerated]
public class LetterHelperAdditionalTests
{
    [Fact]
    public void TryShuffleLetters_WithAsterisk_PreservesAsterisk()
    {
        var result = LetterHelper.ShuffleLetters("A*BC");

        Assert.Contains("*", result);
        Assert.Equal(4, result.Length);
    }

    [Fact]
    public void TryShuffleLetters_WithoutAsterisk_ShufflesLetters()
    {
        var result = LetterHelper.ShuffleLetters("ABC");

        Assert.Equal(3, result.Length);
        Assert.DoesNotContain("*", result);
    }

    [Fact]
    public void TryShuffleLetters_MultipleLetters_ContainsAllLetters()
    {
        const string Input = "ABCDEFGHI";

        var result = LetterHelper.ShuffleLetters(Input);

        Assert.Equal(Input.Length, result.Length);

        foreach (var c in Input)
        {
            Assert.Contains(c, result);
        }
    }
}
