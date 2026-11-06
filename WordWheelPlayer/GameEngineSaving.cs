namespace WordWheelPlayer;

public partial class GameEngine
{
    private void SaveGame()
    {
        var code = SavedGameStore.GenerateCode();

        SavedGames.Save(new SavedGame
        {
            Code = code,
            Answer = AvailableWords.LongestWord ?? string.Empty,
            WordsFound = [.. wordsFoundSoFar]
        });

        DisplayMessage($"Game saved. Your code is {code}");
        DisplayMessage("You will need this code to carry the game on");
    }

    private SavedGame? ChooseSavedGame()
    {
        var savedGame = SavedGames.Load();

        if (savedGame == null)
        {
            return null;
        }

        while (true)
        {
            DisplayMessage("You have a saved game. Do you want to carry it on? (Y/N)");

            switch (ConsoleOperations.ReadInput()?.Trim().ToUpper())
            {
                case null:
                    // End of input isn't a decision, so the saved game is left alone
                    return null;
                case "Y":
                case "YES":
                    if (CodeAccepted(savedGame))
                    {
                        return savedGame;
                    }

                    break;
                case "N":
                case "NO":
                    SavedGames.Delete();
                    DisplayMessage("Saved game deleted");

                    return null;
            }
        }
    }

    private bool CodeAccepted(SavedGame savedGame)
    {
        while (true)
        {
            DisplayMessage("Enter your code (or just press Enter to go back)");

            var code = ConsoleOperations.ReadInput()?.Trim().ToUpper();

            if (string.IsNullOrEmpty(code))
            {
                return false;
            }

            if (code == savedGame.Code)
            {
                return true;
            }

            DisplayMessage("That code is not correct");
        }
    }
}
