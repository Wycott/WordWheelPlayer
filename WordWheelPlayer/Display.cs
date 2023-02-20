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

        private static void DisplayMessage(string text)
        {
            var currentForeground = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Yellow;

            WriteLine(text);

            Console.ForegroundColor = currentForeground;
        }

        private static void DisplayInstructions()
        {
            var currentForeground = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Yellow;

            DisplayInstructionText();

            Console.ForegroundColor = currentForeground;
        }

        private static void DisplayInstructionText()
        {
            WriteLine();
            WriteLine(CenterText(".oO WORD WHEEL Oo."));
            WriteLine();
            WriteLine($"Find as many words of {MinLength} letters or more using the central letter (marked with *)");
            WriteLine();
            WriteLine("The game supports the following commands:");
            WriteLine("\t:LETTERS - to display letters");
            WriteLine("\t:WORDS   - to display words found so far");
            WriteLine("\t:HELP    - display this text");
            WriteLine("\t:EXIT    - to quit");
            WriteLine();
        }

        private static string CenterText(string text)
        {
            var width = 80;

            var padSize = (width - text.Length) / 2;

            return new string(' ',  padSize) + text;

        }
    }
}
