using AiAnnotations;
using WordWheelPlayer.Helpers;

namespace WordWheelPlayer.Test.Helpers;

[AiGenerated]
public class LetterHelperAdditionalTests
{
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
