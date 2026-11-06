namespace WordWheelPlayer.Helpers;

public static class GuessValidator
{
    // Returns the message to show the player, or null if the guess is acceptable
    public static string? FindProblem(string word, string wheelLetters, string keyLetter, List<string> wordsFound, EnglishDictionary dictionary)
    {
        if (EnglishDictionary.EndsInSingleS(word))
        {
            return "Words may not end in S (-SS is allowed)";
        }

        if (word[0] == ':')
        {
            return "Not a valid command";
        }

        if (wordsFound.Contains(word))
        {
            return "That word was already found";
        }

        if (FindInvalidLetter(word, wheelLetters) is { } invalidLetter)
        {
            return $"{invalidLetter} is not a valid letter";
        }

        if (!word.Contains(keyLetter))
        {
            return $"Word must contain the letter {keyLetter}";
        }

        if (FindOverusedLetter(word, wheelLetters) is { } overusedLetter)
        {
            return $"{overusedLetter} has been used too many times";
        }

        if (word.Length < dictionary.MinWordLength)
        {
            return $"Words must be at least {dictionary.MinWordLength} letters long!";
        }

        if (!dictionary.WordIsInDictionary(word))
        {
            return "Word not found in dictionary";
        }

        return null;
    }

    private static char? FindInvalidLetter(string word, string wheelLetters)
    {
        foreach (var letter in word)
        {
            if (!wheelLetters.Contains(letter))
            {
                return letter;
            }
        }

        return null;
    }

    private static char? FindOverusedLetter(string word, string wheelLetters)
    {
        var remainingLetters = wheelLetters.ToList();

        foreach (var letter in word)
        {
            if (!remainingLetters.Remove(letter))
            {
                return letter;
            }
        }

        return null;
    }
}
