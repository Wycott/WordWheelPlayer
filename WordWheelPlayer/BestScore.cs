namespace WordWheelPlayer;

public class BestGame
{
    public int BestScore { get; set; }
    public DateTime BestDate { get; set; }
    public int WordsFound { get; set; }
    public string Letters { get; set; } = string.Empty;
    public string CentralLetter { get; set; } = string.Empty;

    public string WhenAchieved(DateTime gameStartTime)
    {
        return BestDate > gameStartTime ? "Just now" : BestDate.ToString("dd/MM/yyyy HH:mm:ss");
    }
}
