using AiAnnotations;
using Moq;

namespace WordWheelPlayer.Test;

[AiGenerated]
public class GameEngineAdditionalTests
{
    [Theory]
    [InlineData(":W")]
    [InlineData(":L")]
    [InlineData(":H")]
    [InlineData(":COMMANDS")]
    [InlineData(":R")]
    [InlineData(":S")]
    [InlineData(":V")]
    [InlineData(":M")]
    [InlineData(":EASTEREGG")]
    public void Start_ShortCommands_ExecuteCorrectly(string command)
    {
        var mockConsole = new Mock<IGameConsole>();
        mockConsole.SetupSequence(c => c.ReadInput())
            .Returns(command)
            .Returns(":QUIT");

        var gameEngine = new GameEngine(mockConsole.Object);

        gameEngine.Start();

        mockConsole.Verify(c => c.ReadInput(), Times.Exactly(2));
    }

    [Fact]
    public void Start_EmptyInput_ContinuesGame()
    {
        var mockConsole = new Mock<IGameConsole>();
        mockConsole.SetupSequence(c => c.ReadInput())
            .Returns("")
            .Returns(":EXIT");

        var gameEngine = new GameEngine(mockConsole.Object);

        gameEngine.Start();

        mockConsole.Verify(c => c.ReadInput(), Times.Exactly(2));
    }

    [Fact]
    public void Start_NullInput_HandlesGracefully()
    {
        var mockConsole = new Mock<IGameConsole>();
        mockConsole.SetupSequence(c => c.ReadInput())
            .Returns((string?)null)
            .Returns(":QUIT");

        var gameEngine = new GameEngine(mockConsole.Object);

        gameEngine.Start();

        mockConsole.Verify(c => c.ReadInput(), Times.Exactly(2));
    }

    [Fact]
    public void Start_DuplicateWord_ShowsMessage()
    {
        var mockConsole = new Mock<IGameConsole>();
        mockConsole.SetupSequence(c => c.ReadInput())
            .Returns(":WORDS")
            .Returns(":WORDS")
            .Returns(":QUIT");

        var gameEngine = new GameEngine(mockConsole.Object);

        gameEngine.Start();

        mockConsole.Verify(c => c.ReadInput(), Times.Exactly(3));
    }

    [Fact]
    public void Start_SingleCharacterCommand_InvalidCommand()
    {
        var mockConsole = new Mock<IGameConsole>();
        mockConsole.SetupSequence(c => c.ReadInput())
            .Returns(":")
            .Returns(":QUIT");

        var gameEngine = new GameEngine(mockConsole.Object);

        gameEngine.Start();

        mockConsole.Verify(c => c.ReadInput(), Times.Exactly(2));
    }
}
