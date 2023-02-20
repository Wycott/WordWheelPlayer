using static System.Console;

namespace WordWheelPlayer
{
    public partial class GameEngine
    {
        private int DisplayWordsFound()
        {
            var wordCount = 0;

            foreach (var foundWord in wordsFoundSoFar)
            {
                WriteLine(foundWord);
                wordCount++;
            }

            if (wordCount == 0)
            {
                DisplayMessage("No words found yet!");
            }

            return wordCount;
        }

        private static void DisplayWordTotal(int wordCount)
        {
            WriteLine();
            WriteLine($"Total:{wordCount}");
            WriteLine();
        }

        private void DisplayLetters()
        {
            Write(GameLetters);

            WriteLine();
            WriteLine();
        }

        private static void DisplayMessageLines(List<string> textLines)
        {
            var currentForeground = ForegroundColor;

            ForegroundColor = ConsoleColor.Yellow;

            foreach (var line in textLines)
            {
                WriteLine(line);
            }

            ForegroundColor = currentForeground;
        }

        private static void DisplayMessage(string text)
        {
            var lines = new List<string>() { text };

            DisplayMessageLines(lines);
        }

        private static void DisplayInstructions()
        {
            var lines = new List<string>()
            {
                "",
                CenterText(".oO WORD WHEEL Oo."),
                "",
                $"Find as many words of {MinLength} letters or more using the central letter (marked with *)",
                "",
                "The game supports the following commands:",
                "\t:LETTERS - to display letters",
                "\t:WORDS   - to display words found so far",
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
}
