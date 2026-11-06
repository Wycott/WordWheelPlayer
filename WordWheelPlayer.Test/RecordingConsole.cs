namespace WordWheelPlayer.Test;

internal sealed class RecordingConsole : IGameConsole
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
