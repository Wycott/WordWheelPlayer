namespace WordWheelPlayer.Helpers;

public static class LetterHelper
{
    public static string ShuffleLetters(string letters)
    {
        var retVal = string.Empty;
        var shuffleLetters = new List<GameLetter>();

        foreach (var letter in letters)
        {
            if (letter == '*')
            {
                shuffleLetters[^1].MustInclude = true;
            }
            else
            {
                shuffleLetters.Add(new GameLetter { Letter = letter.ToString() });
            }
        }

        foreach (var shuffleLetter in shuffleLetters.OrderBy(_ => Guid.NewGuid().ToString()))
        {
            retVal += shuffleLetter.Letter;

            if (shuffleLetter.MustInclude)
            {
                retVal += '*';
            }
        }

        return retVal;
    }
}