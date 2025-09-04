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
        var expectedPadding = (80 - Input.Length) / 2;
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
        var expectedOutput = new string(' ', 40); // Half of 80

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
        var expectedOutput = input; // No padding needed

        // Act
        var actualOutput = DisplayHelper.CenterText(input);

        // Assert
        Assert.Equal(expectedOutput, actualOutput);
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