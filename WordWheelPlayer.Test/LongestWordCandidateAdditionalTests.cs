using AiAnnotations;

namespace WordWheelPlayer.Test;

[AiGenerated]
public class LongestWordCandidateAdditionalTests
{
    [Fact]
    public void LongestWordCandidate_DefaultValues_AreNull()
    {
        var candidate = new LongestWordCandidate();

        Assert.Null(candidate.LongestWord);
        Assert.Null(candidate.SortBy);
    }

    [Fact]
    public void LongestWordCandidate_EmptyStrings_CanBeSet()
    {
        var candidate = new LongestWordCandidate
        {
            LongestWord = "",
            SortBy = ""
        };

        Assert.Equal("", candidate.LongestWord);
        Assert.Equal("", candidate.SortBy);
    }

    [Fact]
    public void LongestWordCandidate_LongWord_CanBeStored()
    {
        var longWord = new string('A', 100);
        var candidate = new LongestWordCandidate { LongestWord = longWord };

        Assert.Equal(longWord, candidate.LongestWord);
    }
}
