namespace WordWheelPlayer;

public class GameLetter
{
    public required string Letter { get; set; }
    public bool Used { get; set; }
    public bool MustInclude { get; set; }
}
