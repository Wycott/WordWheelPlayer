using System.Text.RegularExpressions;

namespace WordWheelPlayer
{
    public class EnglishDictionary
    {
        private readonly List<string> englishDictionary = new();
        private readonly List<LongestWordCandidate> candidateWords = new();

        private int MinWordLength { get; set; }
        private int MaxWordLength { get; set; }

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
                        candidateWords.Add(new LongestWordCandidate() { LongestWord = candidate, SortBy = Guid.NewGuid().ToString() });
                    }
                }
            }

            var firstCandidateWord = candidateWords.MinBy(x => x.SortBy);
            LongestWord = firstCandidateWord?.LongestWord;

            if (LongestWord != null)
            {
                GameLetters = FindMostCommonLetter(LongestWord);
            }
        }

        public bool WordIsInDictionary(string wordToCheck)
        {
            return englishDictionary.Contains(wordToCheck);
        }

        private List<string> FindMostCommonLetter(string word)
        {
            var retVal = new List<string>();

            int maxVal = 100;
            string currentLetter = string.Empty;
            
            foreach (var letter in word)
            {
                int valueFound = -1;

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
                        //currentLetter = letter.ToString();
                        break;
                    case 'D':
                    case 'G':
                        valueFound = 2;
                        //currentLetter = letter.ToString();
                        break;
                    case 'B':
                    case 'C':
                    case 'M':
                    case 'P':
                        valueFound = 3;
                        //currentLetter = letter.ToString();
                        break;
                    case 'F':
                    case 'H':
                    case 'V':
                    case 'W':
                    case 'Y':
                        valueFound = 4;
                        //currentLetter = letter.ToString();
                        break;

                    case 'K':
                        valueFound = 5;
                        //currentLetter = letter.ToString();
                        break;
                    case 'J':
                    case 'X':
                        valueFound = 8;
                        //currentLetter = letter.ToString();
                        break;
                    case 'Q':
                    case 'Z':
                        valueFound = 10;
                        //currentLetter = letter.ToString();
                        break;
                }

                if (valueFound < maxVal)
                {
                    maxVal = valueFound;
                    currentLetter = letter.ToString();
                }
            }

            retVal.Add(currentLetter);

            bool found = false;

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
}
