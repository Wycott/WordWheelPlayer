using AiAnnotations;

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

    private void DisplayTotals(int wordCount, int score)
    {
        ConsoleOperations.WriteEmptyLine();
        ConsoleOperations.WriteOutput($"Words Found : {wordCount}");
        ConsoleOperations.WriteOutput($"Score       : {score}");
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
            CenterText("------------------"),
            CenterText(".oO WORD WHEEL Oo."),
            CenterText("------------------"),
            "",
            $"Find as many words of {MinLength} letters or more using the central letter (marked with *)",
            $"There will be at least one word that uses all {MaxLength} letters",
            "",
            "The game supports the following commands:",
            "\t:LETTERS - to display letters",
            "\t:WORDS   - to display words found so far",
            "\t:SHUFFLE - shuffle the letters",
            "\t:HELP    - display this text",
            "\t:EXIT    - to quit",
            ""
        };

        DisplayMessageLines(lines);
    }

    private static string CenterText(string text)
    {
        const int Width = 80;

        var padSize = (Width - text.Length) / 2;

        return new string(' ', padSize) + text;
    }
}