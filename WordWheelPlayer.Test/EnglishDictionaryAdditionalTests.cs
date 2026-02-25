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
    public void EnglishDictionary_MinMaxWordLength_SetCorrectly()
    {
        var dictionary = new EnglishDictionary(5, 12);

        Assert.Equal(5, dictionary.MinWordLength);
        Assert.Equal(12, dictionary.MaxWordLength);
    }
}
