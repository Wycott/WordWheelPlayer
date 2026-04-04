using System.Diagnostics.CodeAnalysis;
using static System.Console;

namespace WordWheelPlayer;

[ExcludeFromCodeCoverage]
public class GameConsole : IGameConsole
{
    public GameConsole()
    {
        ForegroundColour = ForegroundColor;
    }

    public ConsoleColor ForegroundColour
    {
        get;

        set
        {
            field = value;
            ForegroundColor = value;
        }
    }

    public void Cls()
    {
        Clear();
    }

    public void WriteEmptyLine()
    {
        WriteLine();
    }

    public void WriteOutput(string message)
    {
        WriteLine(message);
    }

    public string? ReadInput()
    {
        return ReadLine();
    }
}
