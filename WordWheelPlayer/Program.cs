using System.Diagnostics.CodeAnalysis;

namespace WordWheelPlayer;

[ExcludeFromCodeCoverage]
internal class Program
{
    private static void Main()
    {
        var engine = new GameEngine(new GameConsole());
        engine.Start();
    }
}