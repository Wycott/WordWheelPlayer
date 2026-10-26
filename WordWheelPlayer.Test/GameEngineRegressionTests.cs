using System.Reflection;
using AiAnnotations;

namespace WordWheelPlayer.Test;

[AiGenerated]
public class GameEngineRegressionTests
{
    private sealed class RecordingConsole : IGameConsole
    {
        private readonly Queue<string?> inputs = new();

        public List<string> Log { get; } = [];

        public ConsoleColor ForegroundColour { get; set; }

        public void Enqueue(string? input) => inputs.Enqueue(input);

        public void Cls() => Log.Add("CLS");

        public void WriteEmptyLine() => Log.Add("OUT:");

        public void WriteOutput(string message) => Log.Add($"OUT:{message}");

        public string? ReadInput()
        {
            Log.Add("READ");

            // Behave like Console.ReadLine at end of input once the script runs out
            return inputs.Count > 0 ? inputs.Dequeue() : null;
        }
    }

    private static string GetKeyLetter(GameEngine engine)
    {
        var field = typeof(GameEngine).GetField("keyLetter", BindingFlags.NonPublic | BindingFlags.Instance);

        return (string)field!.GetValue(engine)!;
    }

    private static string GetAnswer(RecordingConsole console, GameEngine engine)
    {
        console.Enqueue(":PEEK");
        console.Enqueue(":QUIT");
        engine.Start();

        var peekIndex = console.Log.LastIndexOf("READ") - 1;
        var answer = console.Log[peekIndex][4..];
        console.Log.Clear();

        return answer;
    }

    private static bool EndsInSingleS(string word) => word.EndsWith('S') && !word.EndsWith("SS");

    [Fact]
    public void InitDictionary_NeverPicksAnswerEndingInSingleS()
    {
        var dictionary = new EnglishDictionary(3, 9);

        for (var i = 0; i < 20; i++)
        {
            dictionary.InitDictionary();

            Assert.NotNull(dictionary.LongestWord);
            Assert.False(EndsInSingleS(dictionary.LongestWord), $"{dictionary.LongestWord} cannot be entered");
        }
    }

    [Fact]
    public void Start_TooShortWord_DoesNotLeaveLettersUsed()
    {
        var console = new RecordingConsole();
        var engine = new GameEngine(console);
        var answer = GetAnswer(console, engine);
        var key = GetKeyLetter(engine);

        var other = answer.Remove(answer.IndexOf(key, StringComparison.Ordinal), 1)[0];
        var shortWord = $"{key}{other}";

        if (EndsInSingleS(shortWord))
        {
            shortWord = $"{other}{key}";
        }

        console.Enqueue(shortWord);
        console.Enqueue(answer);
        console.Enqueue(":QUIT");
        engine.Start();

        Assert.Contains("OUT:Words must be at least 3 letters long!", console.Log);
        Assert.Contains("OUT:Score            : 21", console.Log);
    }

    [Fact]
    public void Start_WordNotInDictionary_MessageIsNotClearedAway()
    {
        var console = new RecordingConsole();
        var engine = new GameEngine(console);
        var answer = GetAnswer(console, engine);
        var key = GetKeyLetter(engine)[0];

        var dictionary = new EnglishDictionary(3, 9);
        dictionary.InitDictionary();

        var others = answer.Remove(answer.IndexOf(key), 1);
        var nonWord = (from a in others
                       from b in others
                       let candidate = $"{key}{a}{b}"
                       where a != b || others.Count(c => c == a) > 1
                       where !EndsInSingleS(candidate) && !dictionary.WordIsInDictionary(candidate)
                       select candidate).First();

        console.Enqueue(nonWord);
        console.Enqueue(":QUIT");
        engine.Start();

        var messageIndex = console.Log.IndexOf("OUT:Word not found in dictionary");

        Assert.True(messageIndex >= 0, "Expected the not-found message");
        Assert.DoesNotContain("CLS", console.Log[messageIndex..]);
    }

    [Fact]
    public void Start_EndOfInput_EndsGame()
    {
        var console = new RecordingConsole();
        var engine = new GameEngine(console);

        // No scripted input: the first read returns null, as Console.ReadLine does at end of input
        engine.Start();

        Assert.Equal(1, console.Log.Count(entry => entry == "READ"));
    }
}
