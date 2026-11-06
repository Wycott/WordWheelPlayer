using AiAnnotations;

namespace WordWheelPlayer.Test;

[AiGenerated]
public class EnglishDictionaryAdditionalTests
{
    [Fact]
    public void EnglishDictionary_LongestWord_IsNull_BeforeInit()
    {
        var dictionary = new EnglishDictionary(3, 9);

        Assert.Null(dictionary.LongestWord);
    }

    [Fact]
    public void EnglishDictionary_GameLetters_IsEmpty_BeforeInit()
    {
        var dictionary = new EnglishDictionary(3, 9);

        Assert.Empty(dictionary.GameLetters);
    }

    [Fact]
    public void WordIsInDictionary_EmptyDictionary_ReturnsFalse()
    {
        var dictionary = new EnglishDictionary(3, 9);

        var result = dictionary.WordIsInDictionary("TEST");

        Assert.False(result);
    }

    [Fact]
    public void InitDictionary_SecondCall_DoesNotReloadTheWordFile()
    {
        var wordsFile = Path.Combine(Path.GetTempPath(), $"words-{Guid.NewGuid():N}.txt");

        File.WriteAllLines(wordsFile, ["CATHEDRAL", "CAT"]);

        var dictionary = new EnglishDictionary(3, 9, wordsFile);

        dictionary.InitDictionary();
        File.Delete(wordsFile);
        dictionary.InitDictionary();

        Assert.Equal("CATHEDRAL", dictionary.LongestWord);
        Assert.True(dictionary.WordIsInDictionary("CAT"));
    }

    [Fact]
    public void InitDictionary_RepeatedCalls_PickDifferentAnswers()
    {
        var dictionary = new EnglishDictionary(3, 9);
        var answers = new HashSet<string?>();

        for (var i = 0; i < 20; i++)
        {
            dictionary.InitDictionary();
            answers.Add(dictionary.LongestWord);
        }

        Assert.True(answers.Count > 1, "Each game should get a freshly chosen answer");
    }

    [Fact]
    public void EnglishDictionary_MinMaxWordLength_SetCorrectly()
    {
        var dictionary = new EnglishDictionary(5, 12);

        Assert.Equal(5, dictionary.MinWordLength);
        Assert.Equal(12, dictionary.MaxWordLength);
    }
}
