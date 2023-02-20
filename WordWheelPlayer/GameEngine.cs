using System.Text.RegularExpressions;
using static System.Console;

namespace WordWheelPlayer;

public partial class GameEngine
{
    private const int MinLength = 3;

    private readonly List<string> wordsFoundSoFar = new();

    private readonly List<GameLetter> gameLetters = new();

    private string keyLetter = string.Empty;

    private readonly List<string> englishDictionary = new();

    private string GameLetters { get; set; }

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

    //private readonly List<string> lettersToUse = new()
    //{
    //    "A", // By convention, all words must include this letter
    //    "C",
    //    "E",
    //    "L",
    //    "R",
    //    "W",
    //    "T",
    //    "A",
    //    "U"
    //};

    private readonly List<string> lettersToUse = new()
    {
        "I", // By convention, all words must include this letter
        "M",
        "A",
        "G",
        "N",
        "C",
        "O",
        "R",
        "N"
    };

    public GameEngine()
    {
        DisplayInstructions();
        Init();
    }

    public void Start()
    {
        var quitGame = false;

        while (!quitGame)
        {
            var word = ReadLine();
            WriteLine();

            if (word != null)
            {
                word = word.ToUpper();

                if (word.Substring(0, 1) == ":" && word.Length > 1)
                {
                    switch (word.Substring(1))
                    {
                        case "WORDS":
                            DisplayWordsFound();
                            break;
                        case "LETTERS":
                            DisplayLetters();
                            break;
                        case "HELP":
                            DisplayInstructions();
                            break;
                        case "SHUFFLE":
                            // TODO
                            break;
                        case "QUIT":
                        case "EXIT":
                            quitGame = true;
                            continue;
                    }
                }

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
                    if (word.Length < MinLength)
                    {
                        DisplayMessage($"Words must be at least {MinLength} letters long!");
                        continue;
                    }

                    if (WordIsInDictionary(word) && word.Length >= MinLength)
                    {
                        wordsFoundSoFar.Add(word);
                    }
                }

                ResetLetters();
            }

            wordsFoundSoFar.Sort();

            Clear();

            DisplayLetters();

            var wordCount = DisplayWordsFound();

            DisplayWordTotal(wordCount);
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

        GameLetters = string.Empty;

        foreach (var letter in lettersToUse)
        {
            var gameLetter = new GameLetter()
            {
                Letter = letter,
                Used = false
            };

            GameLetters += letter;

            if (gameLetters.Count == 0)
            {
                gameLetter.MustInclude = true;
                keyLetter = letter;

                GameLetters += "*";
            }

            gameLetters.Add(gameLetter);
        }

        DisplayLetters();
    }

    private void InitDictionary()
    {
        const string RegExPattern = "^[a-zA-Z]+$";

        using var sr = new StreamReader("words.txt");

        while (sr.ReadLine() is { } line)
        {
            var candidate = line.ToUpper();

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