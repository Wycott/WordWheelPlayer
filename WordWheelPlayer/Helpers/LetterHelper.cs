namespace WordWheelPlayer.Helpers;

public static class LetterHelper
{
    public static string ShuffleLetters(string letters)
    {
        return string.Concat(letters.OrderBy(_ => Random.Shared.Next()));
    }
}
