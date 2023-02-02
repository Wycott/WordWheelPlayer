using System.Text.RegularExpressions;

namespace WordWheelPlayer;

public class GameEngine
{
    private const int MinLength = 3;

    private readonly List<string> wordsFoundSoFar = new();

    private readonly List<GameLetter> gameLetters = new();

    private string keyLetter = string.Empty;

    private readonly List<string> englishDictionary = new();

    //private List<string> LettersToUse = new List<string>()
    //{
    //    "I", // By convention, all words must include this letter
    //    "E",
    //    "D",
    //    "F",
    //    "T",
    //    "C",
    //    "E",
    //    "G",
    //    "N"
    //};

    private readonly List<string> lettersToUse = new()
    {
        "A", // By convention, all words must include this letter
        "C",
        "E",
        "L",
        "R",
        "W",
        "T",
        "A",
        "U"
    };

    public GameEngine()
    {
        Init();
    }

    public void Start()
    {
        var word = string.Empty;

        while (word != "-")
        {
            word = Console.ReadLine();
            Console.WriteLine();

            if (word != null)
            {
                word = word.ToUpper();

                if (wordsFoundSoFar.Contains(word) || !word.Contains(keyLetter))
                {
                    continue;
                }

                var letterCount = 0;

                foreach (var guessLetter in word)
                {

                    var gl = gameLetters.FirstOrDefault(x => x.Letter == guessLetter.ToString() && x.Used == false);

                    if (gl == null)
                    {
                        continue;
                    }

                    gl.Used = true;
                    letterCount++;
                }

                if (letterCount == word.Length)
                {
                    if (WordIsInDictionary(word) && word.Length >= MinLength)
                    {
                        wordsFoundSoFar.Add(word);
                    }
                }

                ResetLetters();
            }

            var wordCount = 0;

            wordsFoundSoFar.Sort();

            Console.Clear();

            foreach (var foundWord in wordsFoundSoFar)
            {
                Console.WriteLine(foundWord);
                wordCount++;
            }

            Console.WriteLine();
            Console.WriteLine($"Total:{wordCount}");
            Console.WriteLine();
        }
    }

    private void ResetLetters()
    {
        foreach (var letter in gameLetters)
        {
            letter.Used = false;
        }
    }

    private void Init()
    {
        InitDictionary();

        foreach (var letter in lettersToUse)
        {
            var gameLetter = new GameLetter()
            {
                Letter = letter,
                Used = false
            };

            Console.Write(letter);

            if (gameLetters.Count == 0)
            {
                gameLetter.MustInclude = true;
                keyLetter = letter;

                Console.Write("*");
            }

            gameLetters.Add(gameLetter);
        }

        Console.WriteLine();
    }

    private void InitDictionary()
    {
        using var sr = new StreamReader("words.txt");

        while (sr.ReadLine() is { } line)
        {
            var candidate = line.ToUpper();

            const string RegExPattern = "^[a-zA-Z]+$";

            if (candidate.Length <= lettersToUse.Count &&
                candidate.Length >= MinLength &&
                Regex.IsMatch(candidate, RegExPattern)
               )
            {
                englishDictionary.Add(candidate);
            }
        }
    }

    private bool WordIsInDictionary(string wordToCheck)
    {
        return englishDictionary.Contains(wordToCheck);
    }
}