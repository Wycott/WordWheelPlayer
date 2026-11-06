using AiAnnotations;

namespace WordWheelPlayer.Test;

[AiGenerated]
public sealed class SavedGameTests : IDisposable
{
    private const string ScoreOf21 = "OUT:Score            : 21";
    private const string CarryOnPrompt = "OUT:You have a saved game. Do you want to carry it on? (Y/N)";
    private const string WrongCodeMessage = "OUT:That code is not correct";

    private readonly string savePath = Path.Combine(Path.GetTempPath(), $"SavedGame-{Guid.NewGuid():N}.json");

    private SavedGameStore Store => new(savePath);

    public void Dispose()
    {
        File.Delete(savePath);
    }

    private static string GetCode(RecordingConsole console)
    {
        return console.Log.Last(entry => entry.StartsWith("OUT:Game saved"))[^6..];
    }

    private (string Answer, string Code) PlayAndSave()
    {
        var console = new RecordingConsole();
        var engine = new GameEngine(console, Store);

        console.Enqueue(":PEEK");
        console.Enqueue(":QUIT");
        engine.Start();

        var answer = console.Log[console.Log.LastIndexOf("READ") - 1][4..];

        console.Enqueue(answer);
        console.Enqueue(":SAVE");
        console.Enqueue(":QUIT");
        engine.Start();

        return (answer, GetCode(console));
    }

    private RecordingConsole Resume(params string[] inputs)
    {
        var console = new RecordingConsole();

        foreach (var input in inputs)
        {
            console.Enqueue(input);
        }

        new GameEngine(console, Store).Start();

        return console;
    }

    [Fact]
    public void Save_ShowsCodeAndWritesTheSaveFile()
    {
        var (_, code) = PlayAndSave();

        Assert.Equal(6, code.Length);
        Assert.True(File.Exists(savePath));
    }

    [Fact]
    public void NoSavedGame_DoesNotAskToCarryOn()
    {
        var console = Resume(":QUIT");

        Assert.DoesNotContain(CarryOnPrompt, console.Log);
    }

    [Fact]
    public void CarryOn_WithCorrectCode_RestoresTheGame()
    {
        var (answer, code) = PlayAndSave();

        var console = Resume("Y", code, answer, ":PEEK", ":QUIT");

        Assert.Contains(ScoreOf21, console.Log);
        Assert.Contains("OUT:That word was already found", console.Log);

        // Once in the restored word list and once from :PEEK
        Assert.Equal(2, console.Log.Count(entry => entry == $"OUT:{answer}"));
    }

    [Fact]
    public void CarryOn_AcceptsCodeInLowerCase()
    {
        var (_, code) = PlayAndSave();

        var console = Resume("yes", code.ToLower(), ":QUIT");

        Assert.Contains(ScoreOf21, console.Log);
    }

    [Fact]
    public void CarryOn_KeepsTheSaveFile()
    {
        var (_, code) = PlayAndSave();

        Resume("Y", code, ":QUIT");

        Assert.True(File.Exists(savePath));
    }

    [Fact]
    public void CarryOn_WithWrongCode_AsksAgain()
    {
        var (_, code) = PlayAndSave();

        var console = Resume("Y", "WRONG", code, ":QUIT");

        Assert.Contains(WrongCodeMessage, console.Log);
        Assert.Contains(ScoreOf21, console.Log);
    }

    [Fact]
    public void CarryOn_WithBlankCode_GoesBackToTheQuestion()
    {
        PlayAndSave();

        var console = Resume("Y", "", "N", ":QUIT");

        Assert.Equal(2, console.Log.Count(entry => entry == CarryOnPrompt));
        Assert.False(File.Exists(savePath));
    }

    [Fact]
    public void Decline_DeletesTheSaveAndStartsANewGame()
    {
        PlayAndSave();

        var console = Resume("N", ":SCORE", ":QUIT");

        Assert.False(File.Exists(savePath));
        Assert.DoesNotContain(ScoreOf21, console.Log);
        Assert.Contains("OUT:Score            : 0", console.Log);
    }

    [Fact]
    public void UnrecognisedAnswer_AsksAgain()
    {
        PlayAndSave();

        var console = Resume("MAYBE", "N", ":QUIT");

        Assert.Equal(2, console.Log.Count(entry => entry == CarryOnPrompt));
    }

    [Theory]
    [InlineData]
    [InlineData("Y")]
    public void EndOfInput_KeepsTheSave(params string[] inputs)
    {
        PlayAndSave();

        Resume(inputs);

        Assert.True(File.Exists(savePath));
    }

    [Fact]
    public void SavingAgain_ReplacesTheEarlierSave()
    {
        var console = new RecordingConsole();
        var engine = new GameEngine(console, Store);

        console.Enqueue(":SAVE");
        console.Enqueue(":QUIT");
        engine.Start();

        var firstCode = GetCode(console);

        console.Enqueue(":SAVE");
        console.Enqueue(":QUIT");
        engine.Start();

        var secondCode = GetCode(console);

        var resumed = Resume("Y", firstCode, secondCode, ":QUIT");

        Assert.Single(resumed.Log, WrongCodeMessage);
    }

    [Fact]
    public void CorruptSaveFile_StartsANewGame()
    {
        File.WriteAllText(savePath, "not json");

        var console = Resume(":QUIT");

        Assert.DoesNotContain(CarryOnPrompt, console.Log);
    }
}
