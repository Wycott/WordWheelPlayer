using WordWheelPlayer.Helpers;

namespace WordWheelPlayer;

public partial class GameEngine
{
    private const int MinLength = 3;

    private const int MaxLength = 9;

    private readonly List<string> wordsFoundSoFar = [];

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
        var keepPlaying = true;

        while (keepPlaying)
        {
            var input = ConsoleOperations.ReadInput();

            ConsoleOperations.WriteEmptyLine();

            // Null means end of input (e.g. Ctrl+Z / Ctrl+D or a closed pipe)
            keepPlaying = input != null && HandleInput(input.ToUpper());
        }
    }

    private bool HandleInput(string input)
    {
        if (input is ":X" or ":QUIT" or ":EXIT")
        {
            return false;
        }

        if (input.Length > 0 && !TryRunCommand(input))
        {
            PlayWord(input);
        }

        return true;
    }

    private bool TryRunCommand(string input)
    {
        if (input.Length < 2 || input[0] != ':')
        {
            return false;
        }

        switch (input[1..])
        {
            case "EGG":
            case "EASTEREGG":
                DisplayTease();
                break;
            case "W":
            case "WORDS":
                DisplayWordsFound();
                break;
            case "L":
            case "LETTERS":
                DisplayLetters();
                break;
            case "H":
            case "HELP":
            case "COMMANDS":
                DisplayInstructions();
                break;
            case "R":
            case "RESTART":
                RunPreGame();
                break;
            case "S":
            case "SCORE":
                DisplayBestAndCurrentScore(wordsFoundSoFar.Count, Score);
                break;
            case "T":
            case "TOTALS":
                DisplayTotalsBreakdown();
                break;
            case "SAVE":
                SaveGame();
                break;
            case "C":
            case "COUNTS":
                DisplayCountsByLength();
                break;
            case "V":
            case "VERSION":
                DisplayVersion();
                break;
            case "M":
            case "MIX":
                GameLetters = LetterHelper.ShuffleLetters(GameLetters);
                DisplayLetters();
                break;
            case "PEEK": // Easter egg
                PeekWord();
                break;
            default:
                return false;
        }

        return true;
    }

    private void PlayWord(string word)
    {
        var wheelLetters = GameLetters.Replace("*", "");
        var problem = GuessValidator.FindProblem(word, wheelLetters, keyLetter, wordsFoundSoFar, AvailableWords);

        if (problem != null)
        {
            DisplayMessage(problem);

            return;
        }

        AcceptWord(word);
    }

    private void AcceptWord(string word)
    {
        Score += ScoreHelper.CalculateWordScore(word);
        wordsFoundSoFar.Add(word);
        wordsFoundSoFar.Sort();

        ConsoleOperations.Cls();

        var wordCount = DisplayWordsFound();

        DisplayLetters();

        DisplayBestAndCurrentScore(wordCount, Score);
    }

    private void Init()
    {
        keyLetter = lettersToUse[0];

        var markedLetters = $"{keyLetter}*{string.Concat(lettersToUse.Skip(1))}";

        GameLetters = LetterHelper.ShuffleLetters(markedLetters);
        playableWords = AvailableWords.GetPlayableWords(lettersToUse, keyLetter);

        DisplayInitialLetters();
    }
}
