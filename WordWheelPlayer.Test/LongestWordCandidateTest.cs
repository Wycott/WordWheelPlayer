namespace WordWheelPlayer.Test
{
    public class LongestWordCandidateTest
    {
        [Fact]
        public void WhenSetup_ThenShouldBeAsExpected()
        {
            // Arrange
            const string ExpectedLongestWord = "antidisestablishmentarianism";
            string expectedSortBy = Guid.Empty.ToString();

            // Act
            var lwc = new LongestWordCandidate
            {
                SortBy = expectedSortBy,
                LongestWord = ExpectedLongestWord
            };

            // Assert
            Assert.Equal(ExpectedLongestWord, lwc.LongestWord);
            Assert.Equal(expectedSortBy, lwc.SortBy);
        }
    }
}