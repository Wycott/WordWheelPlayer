using System.Reflection;
using AiAnnotations;
using WordWheelPlayer.Helpers;

namespace WordWheelPlayer;

public partial class GameEngine
{
    private void PeekWord()
    {
        if (AvailableWords.LongestWord != null)
        {
            DisplayMessage(AvailableWords.LongestWord);
        }
    }

    private void DisplayTease()
    {
        DisplayMessage("There is. But can you find it?");
    }

    private int DisplayWordsFound()
    {
        var wordCount = wordsFoundSoFar.Count;

        if (wordCount == 0)
        {
            DisplayMessage("No words found yet!");
            return 0;
        }

        var columns = Math.Max(1, Config.WordDisplayColumns);
        var columnWidth = DisplayHelper.GameTextWidth / columns;

        for (var i = 0; i < wordCount; i += columns)
        {
            var line = string.Empty;

            for (var col = 0; col < columns && i + col < wordCount; col++)
            {
                line += wordsFoundSoFar[i + col].PadRight(columnWidth);
            }

            ConsoleOperations.WriteOutput(line.TrimEnd());
        }

        return wordCount;
    }

    private void DisplayBestAndCurrentScore(int wordCount, int score)
    {
        DisplayTotals(wordCount, score);
        DisplayBestTotals(score);
    }

    private void DisplayTotals(int wordCount, int score)
    {
        ConsoleOperations.WriteEmptyLine();
        ConsoleOperations.WriteOutput($"Words Found      : {wordCount}");
        ConsoleOperations.WriteOutput($"Score            : {score}");
        ConsoleOperations.WriteEmptyLine();
    }

    private void DisplayBestTotals(int score)
    {
        var letters = GameLetters.Replace("*", "");
        var bestGame = BestScoreHelper.UpdateBestScoreIfHigher(score, wordsFoundSoFar.Count, letters, keyLetter);

        ConsoleOperations.WriteOutput($"Best Score       : {bestGame.BestScore}");
        ConsoleOperations.WriteOutput($"Best Score Date  : {bestGame.WhenAchieved(WhenStarted)}");
        ConsoleOperations.WriteEmptyLine();
    }

    private void DisplayInitialLetters()
    {
        DisplayLetters();
        ConsoleOperations.WriteEmptyLine();
    }

    private void DisplayLetters()
    {
        ConsoleOperations.WriteEmptyLine();

        var lettersLines = PrepareDisplayLetters(GameLetters);

        foreach (var line in lettersLines)
        {
            ConsoleOperations.WriteOutput(line);
        }
    }

    private void DisplayVersion()
    {
        var currentForeground = ConsoleOperations.ForegroundColour;

        ConsoleOperations.ForegroundColour = ConsoleColor.White;
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            var exePath = Assembly.GetExecutingAssembly().Location;
            var buildDate = File.GetLastWriteTime(exePath);

            ConsoleOperations.WriteOutput($"Version   : {version}");
            ConsoleOperations.WriteOutput($"Build date: {buildDate}");

            ConsoleOperations.WriteEmptyLine();
        }

        ConsoleOperations.ForegroundColour = currentForeground;
    }

    [AiGenerated("Only works for a specific number of game letters (9)")]
    private static List<string> PrepareDisplayLetters(string gameLetters)
    {
        var frontLoadedLetterSet = FrontLoadCentreLetter(gameLetters);
        var centerChar = frontLoadedLetterSet[0];
        var surroundingChars = frontLoadedLetterSet.Substring(1, 8).ToCharArray();

        return
        [
            $"        {surroundingChars[0]}",
            $"      {surroundingChars[7]}   {surroundingChars[1]}",
            $"    {surroundingChars[6]}   {centerChar}   {surroundingChars[2]}",
            $"      {surroundingChars[5]}   {surroundingChars[3]}",
            $"        {surroundingChars[4]}"
        ];
    }

    private static string FrontLoadCentreLetter(string gameLetters)
    {
        var asteriskIndex = gameLetters.IndexOf('*');

        if (asteriskIndex < 1 || asteriskIndex > gameLetters.Length - 1)
        {
            // No asterisk found or at invalid position — return letters without marker
            return gameLetters.Replace("*", "");
        }

        // The character before the asterisk is the centre letter
        var centreLetter = gameLetters[asteriskIndex - 1];
        var remaining = gameLetters[..(asteriskIndex - 1)] + gameLetters[(asteriskIndex + 1)..];

        return centreLetter + remaining;
    }

    private void DisplayMessageLines(List<string> textLines)
    {
        var currentForeground = ConsoleOperations.ForegroundColour;

        ConsoleOperations.ForegroundColour = ConsoleColor.Yellow;

        foreach (var line in textLines)
        {
            ConsoleOperations.WriteOutput(line);
        }

        ConsoleOperations.ForegroundColour = currentForeground;
    }

    private void DisplayMessage(string text)
    {
        var lines = new List<string> { text };

        DisplayMessageLines(lines);
    }

    private void DisplayInstructions()
    {
        var lines = new List<string>
        {
            "",
            DisplayHelper.CenterText("------------------"),
            DisplayHelper.CenterText(".oO WORD WHEEL Oo."),
            DisplayHelper.CenterText("------------------"),
            "",
            $"Find as many words of {MinLength} letters or more using the central letter",
            "",
            "Words may not end with the letter S",
            "",
            $"There will be at least one word that uses all {MaxLength} letters",
            "",
            "The game supports the following commands:",
            "",
            "\t:LETTERS - to display the letters",
            "\t:WORDS   - to display the words found so far",
            "\t:MIX     - mix up the letters",
            "\t:SCORE   - show the current (and best) score",
            "\t:RESTART - restart the game with new letters",
            "\t:VERSION - show version information",
            "\t:HELP    - display this text",
            "",
            "\t:EXIT    - to quit",
        };

        DisplayMessageLines(lines);
    }
}
