namespace WordWheelPlayer;

public class BestGame
{
    public int BestScore { get; set; }
    public DateTime BestDate { get; set; }
    public DateTime GameStart { get; set; } = DateTime.Now;
    public string WhenAchieved => BestDate > GameStart ? "Just now" : BestDate.ToString("dd/MM/yyyy HH:mm:ss");
}