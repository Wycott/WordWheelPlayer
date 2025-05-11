using WordWheelPlayer.Helpers;
//using static System.Console;

namespace WordWheelPlayer;

public partial class GameEngine
{
    private const int MinLength = 3;

    private const int MaxLength = 9;

    private readonly List<string> wordsFoundSoFar = new();

    private readonly List<GameLetter> gameLetters = new();

    private string keyLetter = string.Empty;

    private string GameLetters { get; set; } = string.Empty;

    private int Score { get; set; }

    private List<int> fibbers = new() { 1, 2, 3, 5, 8, 13, 21 };

    private IGameConsole ConsoleOperations
    {
        get;
    }

    private EnglishDictionary AvailableWords
    {
        get;
    }

    private readonly List<string> lettersToUse;

    public GameEngine(IGameConsole consoleOperations)
    {
        ConsoleOperations = consoleOperations;
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
            var word = ConsoleOperations.ReadInput();
            ConsoleOperations.WriteEmptyLine();

            if (word != null)
            {
                word = word.ToUpper();

                if (word.Length > 1 && word[..1] == ":")
                {
                    switch (word[1..])
                    {
                        case "EGG":
                        case "EASTEREGG":
                            DisplayTease();
                            continue;
                        case "W":
                        case "WORDS":
                            DisplayWordsFound();
                            continue;
                        case "L":
                        case "LETTERS":
                            DisplayLetters();
                            continue;
                        case "H":
                        case "HELP":
                        case "COMMANDS":
                            DisplayInstructions();
                            continue;
                        case "S":
                        case "SHUFFLE":
                            if (GameLetters != null)
                            {
                                GameLetters = LetterHelper.ShuffleLetters(GameLetters);
                            }
                            DisplayLetters();
                            continue;
                        case "PEEK": // Easter egg
                            PeekWord();
                            continue;
                        case "X":
                        case "QUIT":
                        case "EXIT":
                            quitGame = true;
                            continue;
                    }
                }

                if (word.Length == 0)
                {
                    continue;
                }

                if (word.Substring(0, 1) == ":")
                {
                    DisplayMessage("Not a valid command");
                    continue;
                }

                if (!word.Contains(keyLetter))
                {
                    DisplayMessage($"Word must contain the letter {keyLetter}");
                    continue;
                }

                if (wordsFoundSoFar.Contains(word))
                {
                    DisplayMessage("That word was already found");
                    continue;
                }

                if (GameLetters != null && InvalidLetterFound(word, GameLetters, out var badGuy))
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
                        Score += CalculateWordScore(word);
                        wordsFoundSoFar.Add(word);
                    }
                }

                ResetLetters();
            }

            wordsFoundSoFar.Sort();

            ConsoleOperations.Cls();

            DisplayLetters();

            var wordCount = DisplayWordsFound();

            DisplayTotals(wordCount, Score);
        }
    }

    private int CalculateWordScore(string word)
    {
        var index = word.Length - 3;
        var score = fibbers[index];

        return score;
    }

    private void DisplayTease()
    {
        DisplayMessage("There is. But can you find it?");
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