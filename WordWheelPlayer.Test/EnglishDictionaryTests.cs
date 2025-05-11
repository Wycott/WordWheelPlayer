using AiAnnotations;

namespace WordWheelPlayer.Test;

[AiGenerated]
public class EnglishDictionaryTests
{
    [Fact]
    public void EnglishDictionary_ShouldInitializeWithCorrectMinAndMaxWordLengths()
    {
        // Arrange & Act
        var dictionary = new EnglishDictionary(3, 7);

        // Assert
        Assert.Equal(3, dictionary.MinWordLength);
        Assert.Equal(7, dictionary.MaxWordLength);
    }

    [Fact]
    public void WordIsInDictionary_ShouldReturnFalseForWordNotInDictionary()
    {
        // Arrange
        var dictionary = new EnglishDictionary(3, 7);

        // Act
        var result = dictionary.WordIsInDictionary("HELLO");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void WordIsInDictionary_ShouldReturnTrueForWordInDictionary()
    {
        // Arrange
        var dictionary = new EnglishDictionary(3, 7);

        // Simulating dictionary population
        dictionary.InitDictionary();

        dictionary.GameLetters.Add("HELLO");

        // Act
        var result = dictionary.WordIsInDictionary("HELLO");

        // Assert
        Assert.True(result);
    }
}