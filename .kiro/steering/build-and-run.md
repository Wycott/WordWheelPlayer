---
inclusion: always
---

# Build & Run

## Prerequisites

- .NET 10 SDK installed
- `words.txt` file present in the project directory (copied to output on build)

## Build

```bash
dotnet build WordWheelPlayer/WordWheelPlayer.csproj
```

## Run

```bash
dotnet run --project WordWheelPlayer/WordWheelPlayer.csproj
```

## Publish

```bash
dotnet publish WordWheelPlayer/WordWheelPlayer.csproj -c Release
```

## Project Structure

```
WordWheelPlayer/
├── Program.cs                  # Entry point
├── GameEngine.cs               # Core game logic
├── Display.cs                  # UI rendering (partial GameEngine)
├── EnglishDictionary.cs        # Dictionary loading and validation
├── GameConsole.cs              # Console I/O implementation
├── IGameConsole.cs             # Console abstraction interface
├── GameLetter.cs               # Letter model
├── LongestWordCandidate.cs     # Word candidate model
├── BestScore.cs                # Best score model
├── Helpers/
│   ├── BestScoreHelper.cs      # Score persistence
│   ├── DisplayHelper.cs        # Text centering
│   ├── LetterHelper.cs         # Letter shuffling
│   └── ScoreHelper.cs          # Fibonacci scoring
├── words.txt                   # English dictionary file
├── BestGame.json               # Persisted best score (generated at runtime)
└── WordWheelPlayer.csproj      # Project file (.NET 10)
```

## Runtime Files

- `words.txt` — Required. Contains the English word list, one word per line. Copied to output directory automatically via the `.csproj` configuration.
- `BestGame.json` — Generated at runtime. Stores the player's best score and date. Created automatically on first run.

## Notes

- The application is interactive (reads from stdin) so it must be run in a terminal, not as a background process.
- No external services or databases required.
