namespace WordWheelPlayer;

public interface IGameConsole
{
    ConsoleColor ForegroundColour { get; set; }
    void Cls();
    void WriteEmptyLine();
    void WriteOutput(string message);
    string? ReadInput();
}