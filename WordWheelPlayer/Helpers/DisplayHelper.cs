namespace WordWheelPlayer.Helpers;

public static class DisplayHelper
{
    public static string CenterText(string text)
    {
        const int Width = 80;

        if (text.Length > Width)
        {
            return text;
        }

        var padSize = (Width - text.Length) / 2;

        return new string(' ', padSize) + text;
    }
}