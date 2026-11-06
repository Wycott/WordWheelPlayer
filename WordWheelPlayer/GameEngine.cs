using WordWheelPlayer.Helpers;

namespace WordWheelPlayer;

public partial class GameEngine
{
    private const int MinLength = 3;

    private const int MaxLength = 9;

    private readonly List<string> wordsFoundSoFar = [];

    private readonly List<GameLetter> gameLetters = [];

    private string keyLetter = string.Empty;

    private string GameLetters { get; set; } = string.Empty;

    private int Score { get; set; }

    private List<string> playableWords = [];

    private DateTime WhenStarted { get; } = DateTime.Now;

    private List<string> lettersToUse = [];

    private IGameConsole ConsoleOperations
    {
        get;
    }

    private EnglishDictionary AvailableWords
    {
        get;
    }

    private GameConfig Config
    {
        get;
    }

    private SavedGameStore SavedGames
    {
        get;
    }

    public GameEngine(IGameConsole consoleOperations, SavedGameStore? savedGames = null)
    {
        ConsoleOperations = consoleOperations;
        SavedGames = savedGames ?? new SavedGameStore();
        Config = GameConfig.Load();
        AvailableWords = new EnglishDictionary(MinLength, MaxLength);

        var savedGame = ChooseSavedGame();

        RunPreGame(savedGame);

        if (savedGame != null)
        {
            DisplayWordsFound();
            DisplayBestAndCurrentScore(wordsFoundSoFar.Count, Score);
        }
    }

    private void RunPreGame(SavedGame? savedGame = null)
    {
        gameLetters.Clear();
        Score = 0;
        wordsFoundSoFar.Clear();
        AvailableWords.InitDictionary();

        if (savedGame != null)
        {
            AvailableWords.RestoreAnswer(savedGame.Answer);
            wordsFoundSoFar.AddRange(savedGame.WordsFound);
            Score = wordsFoundSoFar.Sum(ScoreHelper.CalculateWordScore);
        }

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

            if (word == null)
            {
                // End of input (e.g. Ctrl+Z / Ctrl+D or a closed pipe)
                quitGame = true;
                continue;
            }

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
                    case "R":
                    case "RESTART":
                        RunPreGame();
                        continue;
                    case "S":
                    case "SCORE":
                        DisplayBestAndCurrentScore(wordsFoundSoFar.Count, Score);
                        continue;
                    case "T":
                    case "TOTALS":
                        DisplayTotalsBreakdown();
                        continue;
                    case "SAVE":
                        SaveGame();
                        continue;
                    case "C":
                    case "COUNTS":
                        DisplayCountsByLength();
                        continue;
                    case "V":
                    case "VERSION":
                        DisplayVersion();
                        continue;
                    case "M":
                    case "MIX":
                        GameLetters = LetterHelper.ShuffleLetters(GameLetters);
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

            if (EnglishDictionary.EndsInSingleS(word))
            {
                DisplayMessage("Words may not end in S (-SS is allowed)");
                continue;
            }

            if (word[..1] == ":")
            {
                DisplayMessage("Not a valid command");
                continue;
            }

            if (wordsFoundSoFar.Contains(word))
            {
                DisplayMessage("That word was already found");
                continue;
            }

            if (InvalidLetterFound(word, GameLetters.Replace("*", ""), out var badGuy))
            {
                DisplayMessage($"{badGuy} is not a valid letter");
                continue;
            }

            if (!word.Contains(keyLetter))
            {
                DisplayMessage($"Word must contain the letter {keyLetter}");
                continue;
            }

            char? overusedLetter = null;

            foreach (var guessLetter in word)
            {
                var gl = gameLetters.FirstOrDefault(x => x.Letter == guessLetter.ToString() && x.Used == false);

                if (gl == null)
                {
                    overusedLetter = guessLetter;
                    break;
                }

                gl.Used = true;
            }

            // Reset before any early exit so a rejected guess can't leave letters marked as used
            ResetLetters();

            if (overusedLetter != null)
            {
                DisplayMessage($"{overusedLetter} has been used too many times");
                continue;
            }

            if (word.Length < MinLength)
            {
                DisplayMessage($"Words must be at least {MinLength} letters long!");
                continue;
            }

            if (!AvailableWords.WordIsInDictionary(word))
            {
                DisplayMessage("Word not found in dictionary");
                continue;
            }

            Score += ScoreHelper.CalculateWordScore(word);
            wordsFoundSoFar.Add(word);

            wordsFoundSoFar.Sort();

            ConsoleOperations.Cls();

            var wordCount = DisplayWordsFound();

            DisplayLetters();

            DisplayBestAndCurrentScore(wordCount, Score);
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
        var sb = new System.Text.StringBuilder();

        foreach (var letter in lettersToUse)
        {
            var gameLetter = new GameLetter
            {
                Letter = letter,
                Used = false
            };

            sb.Append(letter);

            if (gameLetters.Count == 0)
            {
                gameLetter.MustInclude = true;
                keyLetter = letter;

                sb.Append('*');
            }

            gameLetters.Add(gameLetter);
        }

        GameLetters = LetterHelper.ShuffleLetters(sb.ToString());
        playableWords = AvailableWords.GetPlayableWords(lettersToUse, keyLetter);

        DisplayInitialLetters();
    }
}
