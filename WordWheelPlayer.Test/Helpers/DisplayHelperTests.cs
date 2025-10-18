using AiAnnotations;
using AiAnnotations.Types;
using WordWheelPlayer.Helpers;

namespace WordWheelPlayer.Test.Helpers;

[AiGenerated(Authorship.MostlyAi)]
public class DisplayHelperTests
{
    [Fact]
    public void CenterText_ShouldCenterTextCorrectly()
    {
        // Arrange
        const string Input = "Hello";

        var expectedPadding = (DisplayHelper.GameTextWidth - Input.Length) / 2;
        var expectedOutput = new string(' ', expectedPadding) + Input;

        // Act
        var actualOutput = DisplayHelper.CenterText(Input);

        // Assert
        Assert.Equal(expectedOutput, actualOutput);
    }

    [Fact]
    public void CenterText_ShouldHandleEmptyString()
    {
        // Arrange
        const string Input = "";

        var expectedOutput = new string(' ', DisplayHelper.GameTextWidth / 2); // Half of width

        // Act
        var actualOutput = DisplayHelper.CenterText(Input);

        // Assert
        Assert.Equal(expectedOutput, actualOutput);
    }

    [Fact]
    public void CenterText_ShouldHandleLongText()
    {
        // Arrange
        var input = new string('X', 80); // Exactly full width

        // Act
        var actualOutput = DisplayHelper.CenterText(input);

        // Assert
        Assert.Equal(input, actualOutput);
    }

    [Fact]
    public void CenterText_ShouldHandleTextExceedingWidth()
    {
        // Arrange
        var input = new string('X', 100); // More than 80 characters

        // Act
        var actualOutput = DisplayHelper.CenterText(input);

        // Assert
        Assert.Equal(input, actualOutput); // Should return as-is
    }
}