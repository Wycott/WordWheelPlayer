namespace WordWheelPlayer.Test;

public class LongestWordCandidateTest
{
    [Fact]
    public void WhenSetup_ThenShouldBeAsExpected()
    {
        // Arrange
        const string ExpectedLongestWord = "antidisestablishmentarianism";
        const int ExpectedSortBy = 42;

        // Act
        var lwc = new LongestWordCandidate
        {
            SortBy = ExpectedSortBy,
            LongestWord = ExpectedLongestWord
        };

        // Assert
        Assert.Equal(ExpectedLongestWord, lwc.LongestWord);
        Assert.Equal(ExpectedSortBy, lwc.SortBy);
    }
}
