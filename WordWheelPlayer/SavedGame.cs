namespace WordWheelPlayer;

public class SavedGame
{
    public string Code { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public List<string> WordsFound { get; set; } = [];
}
