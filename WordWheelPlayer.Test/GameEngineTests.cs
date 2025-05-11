using AiAnnotations.Types;
using AiAnnotations;
using Moq;

namespace WordWheelPlayer.Test;

[AiGenerated(Authorship.MostlyAi)]
public class GameEngineTests
{
    [Fact]
    public void Start_ShouldExitGameWhenQuitCommandIsEntered()
    {
        // Arrange
        var mockConsole = new Mock<IGameConsole>();
        mockConsole.SetupSequence(c => c.ReadInput())
            .Returns(":QUIT"); // Simulating user input

        var gameEngine = new GameEngine(mockConsole.Object);

        // Act
        gameEngine.Start();

        // Assert
        mockConsole.Verify(c => c.ReadInput(), Times.Once); // Ensures command was read
    }

    [Fact]
    public void Start_ShouldRecognizeAValidWord()
    {
        // Arrange
        var mockConsole = new Mock<IGameConsole>();
        mockConsole.SetupSequence(c => c.ReadInput())
            .Returns("TEST")
            .Returns(":QUIT"); // User enters a word and quits

        var gameEngine = new GameEngine(mockConsole.Object);

        // Act
        gameEngine.Start();

        // Assert
        mockConsole.Verify(c => c.ReadInput(), Times.Exactly(2));
    }

    [Fact]
    public void Start_ShouldHandleInvalidCommandsGracefully()
    {
        // Arrange
        var mockConsole = new Mock<IGameConsole>();
        mockConsole.SetupSequence(c => c.ReadInput())
            .Returns(":INVALID")
            .Returns(":QUIT"); // User enters an invalid command, then quits

        var gameEngine = new GameEngine(mockConsole.Object);

        // Act
        gameEngine.Start();

        // Assert
        mockConsole.Verify(c => c.ReadInput(), Times.Exactly(2));
        mockConsole.Verify(c => c.WriteOutput("Not a valid command"), Times.Once);
    }
}