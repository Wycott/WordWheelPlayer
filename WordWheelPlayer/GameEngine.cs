using WordWheelPlayer.Helpers;
using static System.Console;

namespace WordWheelPlayer;

public partial class GameEngine
{
    private const int MinLength = 3;

    private const int MaxLength = 9;

    private readonly List<string> wordsFoundSoFar = new();

    private readonly List<GameLetter> gameLetters = new();

    private string keyLetter = string.Empty;

    private string? GameLetters { get; set; }

    private EnglishDictionary AvailableWords
    {
        get;
    }

    private readonly List<string> lettersToUse;

    public GameEngine()
    {
        AvailableWords = new EnglishDictionary(MinLength, MaxLength);
        AvailableWords.InitDictionary();
        lettersToUse = AvailableWords.GameLetters;
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

                if (word.Length > 1 && word[..1] == ":")
                {
                    switch (word[1..])
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
                            if (GameLetters != null)
                            {
                                GameLetters = LetterHelper.ShuffleLetters(GameLetters);
                            }
                            DisplayLetters();
                            break;
                        case "PEEK": // Easter egg
                            PeekWord();
                            break;
                        case "QUIT":
                        case "EXIT":
                            quitGame = true;
                            continue;
                    }
                }

                if (!word.Contains(keyLetter))
                {
                    DisplayMessage($"Word must contain the letter {keyLetter}");
                    continue;
                }

                if (wordsFoundSoFar.Contains(word))
                {
                    DisplayMessage($"That word was already found");
                    continue;
                }

                if (InvalidLetterFound(word, GameLetters, out var badGuy))
                {
                    DisplayMessage($"{badGuy} is not a valid letter");
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

                    if (AvailableWords.WordIsInDictionary(word) && word.Length >= MinLength)
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

    private static bool InvalidLetterFound(string guessedWord, string validLetters, out char invalidLetter)
    {
        invalidLetter = ' ';
        foreach (var letter in guessedWord)
        {
            if (!validLetters.Contains(letter))
            {
                invalidLetter = letter;
                return true;
            }
        }

        return false;
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
        GameLetters = string.Empty;

        foreach (var letter in lettersToUse)
        {
            var gameLetter = new GameLetter
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

        GameLetters = LetterHelper.ShuffleLetters(GameLetters);

        DisplayLetters();
    }
}