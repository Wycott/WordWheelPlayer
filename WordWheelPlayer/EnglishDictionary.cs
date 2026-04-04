using System.Text.RegularExpressions;

namespace WordWheelPlayer;

public class EnglishDictionary(int minWordLength, int maxWordLength)
{
    public string? LongestWord { get; set; }
    public List<string> GameLetters = [];

    private readonly List<string> englishDictionary = [];
    private readonly List<LongestWordCandidate> candidateWords = [];

    public int MinWordLength { get; } = minWordLength;
    public int MaxWordLength { get; } = maxWordLength;

    public void InitDictionary()
    {
        candidateWords.Clear();

        const string RegExPattern = "^[a-zA-Z]+$";

        using var sr = new StreamReader("words.txt");

        while (sr.ReadLine() is { } line)
        {
            var candidate = line.ToUpper();

            if (candidate.Length > MaxWordLength ||
                candidate.Length < MinWordLength ||
                !Regex.IsMatch(candidate, RegExPattern))
            {
                continue;
            }

            englishDictionary.Add(candidate);

            if (candidate.Length == MaxWordLength)
            {
                candidateWords.Add(new LongestWordCandidate { LongestWord = candidate, SortBy = Guid.NewGuid().ToString() });
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
