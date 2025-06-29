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
        var wordCount = 0;

        foreach (var foundWord in wordsFoundSoFar)
        {
            ConsoleOperations.WriteOutput(foundWord);
            wordCount++;
        }

        if (wordCount == 0)
        {
            DisplayMessage("No words found yet!");
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
        var bestGame = BestScoreHelper.CheckForBestScore(score);

        ConsoleOperations.WriteOutput($"Best Score       : {bestGame.BestScore}");
        ConsoleOperations.WriteOutput($"Best Score Date  : {bestGame.WhenAchieved}");
        ConsoleOperations.WriteEmptyLine();
    }

    private void DisplayLetters()
    {
        var lettersLines = PrepareDisplayLetters(GameLetters);

        foreach (var line in lettersLines)
        {
            ConsoleOperations.WriteOutput(line);
        }

        ConsoleOperations.WriteEmptyLine();
        ConsoleOperations.WriteEmptyLine();
    }

    [AiGenerated("Only works for a specific number of game letters (9)")]
    private static List<string> PrepareDisplayLetters(string gameLetters)
    {
        var frontLoadedLetterSet = FrontLoadCentreLetter(gameLetters);
        var centerChar = frontLoadedLetterSet[0];
        var surroundingChars = frontLoadedLetterSet.Substring(1, 8).ToCharArray();

        return new List<string>
        {
            $"        {surroundingChars[0]}",
            $"      {surroundingChars[7]}   {surroundingChars[1]}",
            $"    {surroundingChars[6]}   {centerChar}   {surroundingChars[2]}",
            $"      {surroundingChars[5]}   {surroundingChars[3]}",
            $"        {surroundingChars[4]}"
        };
    }

    [AiGenerated]
    private static string FrontLoadCentreLetter(string gameLetters)
    {
        var asteriskIndex = gameLetters.IndexOf('*');

        if (asteriskIndex <= 0 || asteriskIndex == gameLetters.Length - 1)
        {
            return gameLetters;
        }

        var beforeAsterisk = gameLetters.Substring(asteriskIndex - 1, 1);
        var afterAsterisk = gameLetters.Substring(0, asteriskIndex - 1) + gameLetters.Substring(asteriskIndex + 1);

        return beforeAsterisk + afterAsterisk;
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
            $"There will be at least one word that uses all {MaxLength} letters",
            "",
            "The game supports the following commands:",
            "\t:LETTERS - to display the letters",
            "\t:WORDS   - to display the words found so far",
            "\t:MIX     - mix up the letters",
            "\t:SCORE   - show the current (and best) score",
            "\t:HELP    - display this text",
            "\t:EXIT    - to quit",
            ""
        };

        DisplayMessageLines(lines);
    }
}