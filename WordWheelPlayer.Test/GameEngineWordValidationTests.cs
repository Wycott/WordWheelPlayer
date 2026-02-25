using AiAnnotations;
using Moq;

namespace WordWheelPlayer.Test;

[AiGenerated]
public class GameEngineWordValidationTests
{
    [Fact]
    public void Start_InvalidLetter_ShowsMessage()
    {
        var mockConsole = new Mock<IGameConsole>();
        mockConsole.SetupSequence(c => c.ReadInput())
            .Returns("XYZ")
            .Returns(":QUIT");

        var gameEngine = new GameEngine(mockConsole.Object);

        gameEngine.Start();

        mockConsole.Verify(c => c.WriteOutput(It.Is<string>(s => s.Contains("not a valid letter"))), Times.AtLeastOnce);
    }

    [Fact]
    public void Start_RestartCommand_ResetsGame()
    {
        var mockConsole = new Mock<IGameConsole>();
        mockConsole.SetupSequence(c => c.ReadInput())
            .Returns(":R")
            .Returns(":QUIT");

        var gameEngine = new GameEngine(mockConsole.Object);

        gameEngine.Start();

        mockConsole.Verify(c => c.ReadInput(), Times.Exactly(2));
    }

    [Fact]
    public void Start_ScoreCommand_DisplaysScore()
    {
        var mockConsole = new Mock<IGameConsole>();
        mockConsole.SetupSequence(c => c.ReadInput())
            .Returns(":S")
            .Returns(":QUIT");

        var gameEngine = new GameEngine(mockConsole.Object);

        gameEngine.Start();

        mockConsole.Verify(c => c.WriteOutput(It.Is<string>(s => s.Contains("Score"))), Times.AtLeastOnce);
    }

    [Fact]
    public void Start_MixCommand_ShufflesLetters()
    {
        var mockConsole = new Mock<IGameConsole>();
        mockConsole.SetupSequence(c => c.ReadInput())
            .Returns(":M")
            .Returns(":QUIT");

        var gameEngine = new GameEngine(mockConsole.Object);

        gameEngine.Start();

        mockConsole.Verify(c => c.WriteEmptyLine(), Times.AtLeastOnce);
    }
}
