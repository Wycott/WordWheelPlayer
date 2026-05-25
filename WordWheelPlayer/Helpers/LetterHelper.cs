using System.Text;

namespace WordWheelPlayer.Helpers;

public static class LetterHelper
{
    public static string ShuffleLetters(string letters)
    {
        var shuffleLetters = new List<GameLetter>();

        foreach (var letter in letters)
        {
            if (letter == '*')
            {
                shuffleLetters[^1].MustInclude = true;
            }
            else
            {
                var newLetter = new GameLetter { Letter = letter.ToString() };
                shuffleLetters.Add(newLetter);
            }
        }

        var sb = new StringBuilder();

        foreach (var shuffleLetter in shuffleLetters.OrderBy(_ => Random.Shared.Next()))
        {
            sb.Append(shuffleLetter.Letter);

            if (shuffleLetter.MustInclude)
            {
                sb.Append('*');
            }
        }

        return sb.ToString();
    }
}
