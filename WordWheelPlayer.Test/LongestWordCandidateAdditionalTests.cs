using AiAnnotations;

namespace WordWheelPlayer.Test;

[AiGenerated]
public class LongestWordCandidateAdditionalTests
{
    [Fact]
    public void LongestWordCandidate_DefaultValues()
    {
        var candidate = new LongestWordCandidate();

        Assert.Null(candidate.LongestWord);
        Assert.Equal(0, candidate.SortBy);
    }

    [Fact]
    public void LongestWordCandidate_Values_CanBeSet()
    {
        var candidate = new LongestWordCandidate
        {
            LongestWord = "EDUCATION",
            SortBy = 99
        };

        Assert.Equal("EDUCATION", candidate.LongestWord);
        Assert.Equal(99, candidate.SortBy);
    }

    [Fact]
    public void LongestWordCandidate_LongWord_CanBeStored()
    {
        var longWord = new string('A', 100);
        var candidate = new LongestWordCandidate { LongestWord = longWord };

        Assert.Equal(longWord, candidate.LongestWord);
    }
}
