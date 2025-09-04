namespace WordWheelPlayer;

public class BestGame
{
    public int BestScore { get; set; }
    public DateTime BestDate { get; set; }
    public string WhenAchieved(DateTime gameStartTime)
    {
        return BestDate > gameStartTime ? "Just now" : BestDate.ToString("dd/MM/yyyy HH:mm:ss");
    }
}