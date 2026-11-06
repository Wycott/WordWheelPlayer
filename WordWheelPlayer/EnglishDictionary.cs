using System.Text.RegularExpressions;

namespace WordWheelPlayer;

public partial class EnglishDictionary(int minWordLength, int maxWordLength, string wordsFilePath = "words.txt")
{
    public string? LongestWord { get; set; }
    public List<string> GameLetters { get; set; } = [];

    private readonly HashSet<string> englishDictionary = [];
    private readonly List<string> candidateWords = [];

    public int MinWordLength { get; } = minWordLength;
    public int MaxWordLength { get; } = maxWordLength;

    [GeneratedRegex("^[a-zA-Z]+$")]
    private static partial Regex AlphaOnly();

    public void InitDictionary()
    {
        if (englishDictionary.Count == 0)
        {
            LoadWords();
        }

        if (candidateWords.Count > 0)
        {
            RestoreAnswer(candidateWords[Random.Shared.Next(candidateWords.Count)]);
        }
    }

    private void LoadWords()
    {
        using var sr = new StreamReader(wordsFilePath);

        while (sr.ReadLine() is { } line)
        {
            var candidate = line.ToUpper();

            if (candidate.Length > MaxWordLength ||
                candidate.Length < MinWordLength ||
                !AlphaOnly().IsMatch(candidate) ||
                !englishDictionary.Add(candidate))
            {
                continue;
            }

            // Words ending in a single S can't be entered, so they can't be the answer
            if (candidate.Length == MaxWordLength && !EndsInSingleS(candidate))
            {
                candidateWords.Add(candidate);
            }
        }
    }

    public void RestoreAnswer(string answer)
    {
        LongestWord = answer;
        GameLetters = ArrangeLettersForGame(answer);
    }

    public bool WordIsInDictionary(string wordToCheck)
    {
        return englishDictionary.Contains(wordToCheck);
    }

    public static bool EndsInSingleS(string word)
    {
        return word.EndsWith('S') && !word.EndsWith("SS");
    }

    public List<string> GetPlayableWords(IEnumerable<string> letters, string keyLetter)
    {
        var availableLetters = string.Concat(letters);

        return englishDictionary
            .Where(word => word.Contains(keyLetter) && !EndsInSingleS(word) && CanBeMadeFrom(word, availableLetters))
            .ToList();
    }

    private static bool CanBeMadeFrom(string word, string availableLetters)
    {
        var remainingLetters = availableLetters.ToList();

        return word.All(remainingLetters.Remove);
    }

    private static List<string> ArrangeLettersForGame(string word)
    {
        var retVal = new List<string>();

        var maxVal = 100;
        var currentLetter = string.Empty;

        // Pick the most common letter using scrabble weighting as the central letter
        // In practice, the last instance of such a letter will be used
        // Suspect (but have not proved) that this will always have a score of 1
        foreach (var letter in word)
        {
            var valueFound = letter switch
            {
                'A' or 'E' or 'I' or 'O' or 'U' or 'L' or 'N' or 'S' or 'T' or 'R' => 1,
                'D' or 'G' => 2,
                'B' or 'C' or 'M' or 'P' => 3,
                'F' or 'H' or 'V' or 'W' or 'Y' => 4,
                'K' => 5,
                'J' or 'X' => 8,
                'Q' or 'Z' => 10,
                _ => -1
            };

            if (valueFound >= maxVal)
            {
                continue;
            }

            maxVal = valueFound;
            currentLetter = letter.ToString();
        }

        // Add the central letter
        retVal.Add(currentLetter);

        var found = false;

        // And then add the rest omitting the already added letter
        foreach (var letter in word)
        {
            var stringLetter = letter.ToString();

            if (stringLetter != currentLetter)
            {
                retVal.Add(stringLetter);
                continue;
            }

            if (!found)
            {
                found = true;
            }
            else
            {
                retVal.Add(stringLetter);
            }
        }

        return retVal;
    }
}
