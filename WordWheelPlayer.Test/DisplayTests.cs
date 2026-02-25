using AiAnnotations;
using Moq;

namespace WordWheelPlayer.Test;

[AiGenerated]
public class DisplayTests
{
    [Fact]
    public void GameEngine_DisplaysInstructions_OnStartup()
    {
        var mockConsole = new Mock<IGameConsole>();
        mockConsole.Setup(c => c.ReadInput()).Returns(":QUIT");

        var gameEngine = new GameEngine(mockConsole.Object);

        mockConsole.Verify(c => c.WriteOutput(It.IsAny<string>()), Times.AtLeastOnce);
    }

    [Fact]
    public void GameEngine_DisplaysLetters_OnInit()
    {
        var mockConsole = new Mock<IGameConsole>();
        mockConsole.Setup(c => c.ReadInput()).Returns(":QUIT");

        var gameEngine = new GameEngine(mockConsole.Object);

        mockConsole.Verify(c => c.WriteEmptyLine(), Times.AtLeastOnce);
    }

    [Fact]
    public void GameEngine_ChangesColor_WhenDisplayingVersion()
    {
        var mockConsole = new Mock<IGameConsole>();
        mockConsole.SetupSequence(c => c.ReadInput())
            .Returns(":VERSION")
            .Returns(":QUIT");
        mockConsole.SetupProperty(c => c.ForegroundColour);

        var gameEngine = new GameEngine(mockConsole.Object);

        gameEngine.Start();

        mockConsole.VerifySet(c => c.ForegroundColour = ConsoleColor.White, Times.AtLeastOnce);
    }
}
