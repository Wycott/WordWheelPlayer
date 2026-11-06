using AiAnnotations;
using WordWheelPlayer.Helpers;

namespace WordWheelPlayer.Test.Helpers;

[AiGenerated]
public class GuessValidatorTests
{
    private const string WheelLetters = "CATHEDRAL";
    private const string KeyLetter = "A";

    private static readonly EnglishDictionary Dictionary = LoadDictionary();

    private static EnglishDictionary LoadDictionary()
    {
        var dictionary = new EnglishDictionary(3, 9);
        dictionary.InitDictionary();

        return dictionary;
    }

    private static string? FindProblem(string word)
    {
        return GuessValidator.FindProblem(word, WheelLetters, KeyLetter, ["CHAT"], Dictionary);
    }

    [Theory]
    [InlineData("CATHEDRAL")]
    [InlineData("CAT")]
    [InlineData("ALARTED", "Word not found in dictionary")]
    [InlineData("CATS", "Words may not end in S (-SS is allowed)")]
    [InlineData(":NOPE", "Not a valid command")]
    [InlineData(":", "Not a valid command")]
    [InlineData("CHAT", "That word was already found")]
    [InlineData("ZEBRA", "Z is not a valid letter")]
    [InlineData("HELD", "Word must contain the letter A")]
    [InlineData("TATTLE", "T has been used too many times")]
    [InlineData("AT", "Words must be at least 3 letters long!")]
    public void FindProblem_ReturnsExpectedMessage(string word, string? expected = null)
    {
        Assert.Equal(expected, FindProblem(word));
    }

    [Fact]
    public void FindProblem_AllowsARepeatedLetterThatIsInTheWheelTwice()
    {
        Assert.Null(FindProblem("ALTAR"));
    }

    [Fact]
    public void FindProblem_DoesNotUseUpLettersBetweenGuesses()
    {
        Assert.Equal("T has been used too many times", FindProblem("TATTLE"));
        Assert.Null(FindProblem("CATHEDRAL"));
    }
}
