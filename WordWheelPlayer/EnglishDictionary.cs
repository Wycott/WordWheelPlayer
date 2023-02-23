using System.Text.RegularExpressions;

namespace WordWheelPlayer;

public class EnglishDictionary
{
    private readonly List<string> englishDictionary = new();
    private readonly List<LongestWordCandidate> candidateWords = new();

    private int MinWordLength { get; }
    private int MaxWordLength { get; }

    public string? LongestWord { get; set; }

    public List<string> GameLetters = new();

    public EnglishDictionary(int minWordLength, int maxWordLength)
    {
        MinWordLength = minWordLength;
        MaxWordLength = maxWordLength;
    }

    public void InitDictionary()
    {
        const string RegExPattern = "^[a-zA-Z]+$";

        using var sr = new StreamReader("words.txt");

        while (sr.ReadLine() is { } line)
        {
            var candidate = line.ToUpper();

            if (candidate.Length <= MaxWordLength &&
                candidate.Length >= MinWordLength &&
                Regex.IsMatch(candidate, RegExPattern)
               )
            {
                englishDictionary.Add(candidate);

                if (candidate.Length == MaxWordLength)
                {
                    candidateWords.Add(new LongestWordCandidate { LongestWord = candidate, SortBy = Guid.NewGuid().ToString() });
                }
            }
        }

        var firstCandidateWord = candidateWords.MinBy(x => x.SortBy);
        LongestWord = firstCandidateWord?.LongestWord;

        if (LongestWord != null)
        {
            GameLetters = ArrangeLettersForGame(LongestWord);
        }
    }

    public bool WordIsInDictionary(string wordToCheck)
    {
        return englishDictionary.Contains(wordToCheck);
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
            var valueFound = -1;

            switch (letter)
            {
                case 'A':
                case 'E':
                case 'I':
                case 'O':
                case 'U':
                case 'L':
                case 'N':
                case 'S':
                case 'T':
                case 'R':
                    valueFound = 1;
                    break;
                case 'D':
                case 'G':
                    valueFound = 2;
                    break;
                case 'B':
                case 'C':
                case 'M':
                case 'P':
                    valueFound = 3;
                    break;
                case 'F':
                case 'H':
                case 'V':
                case 'W':
                case 'Y':
                    valueFound = 4;
                    break;
                case 'K':
                    valueFound = 5;
                    break;
                case 'J':
                case 'X':
                    valueFound = 8;
                    break;
                case 'Q':
                case 'Z':
                    valueFound = 10;
                    break;
            }

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