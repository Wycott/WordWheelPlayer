namespace WordWheelPlayer.Helpers;

public static class DisplayHelper
{
    public static int GameTextWidth => 64;

    public static string CenterText(string text)
    {
        if (text.Length > GameTextWidth)
        {
            return text;
        }

        var padSize = (GameTextWidth - text.Length) / 2;

        return new string(' ', padSize) + text;
    }
}