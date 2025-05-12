namespace WordWheelPlayer.Helpers
{
    public static class ScoreHelper
    {
        private static readonly List<int> FibonacciNumbers = new() { 1, 2, 3, 5, 8, 13, 21 };

        public static int CalculateWordScore(string word)
        {
            const int MinWordSize = 3;

            if (word.Length < MinWordSize)
            {
                return 0;
            }

            var index = word.Length - MinWordSize;
            var score = FibonacciNumbers[index];

            return score;
        }
    }
}
