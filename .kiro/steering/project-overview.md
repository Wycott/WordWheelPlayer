---
inclusion: always
---

# WordWheelPlayer — Project Overview

## Purpose

WordWheelPlayer is a .NET 10 console application that simulates the word wheel puzzle found in newspapers. The player is given 9 letters arranged in a wheel pattern with a central "key" letter. The goal is to find as many valid English words as possible (3–9 letters) that include the key letter. At least one 9-letter word always exists.

## Architecture

### Entry Point

`Program.cs` creates a `GameEngine` with a concrete `GameConsole` and calls `Start()`.

### Core Components

| File | Responsibility |
|------|---------------|
| `GameEngine.cs` | Game loop, input validation, command dispatch, state management |
| `Display.cs` | Partial class of `GameEngine` — all rendering and UI output |
| `EnglishDictionary.cs` | Loads `words.txt`, filters by length, selects the 9-letter seed word, validates guesses |
| `GameConsole.cs` | Concrete `IGameConsole` wrapping `System.Console` |
| `IGameConsole.cs` | Abstraction for console I/O (enables testability) |

### Data Models

| Class | Purpose |
|-------|---------|
| `GameLetter` | Tracks a letter, whether it's been used this turn, and whether it's the key letter |
| `LongestWordCandidate` | Holds a 9-letter word candidate with a random sort key for selection |
| `BestGame` | Persisted best score and date achieved |

### Helpers (`Helpers/`)

| Helper | Purpose |
|--------|---------|
| `BestScoreHelper` | Loads/saves best score from `BestGame.json` |
| `DisplayHelper` | Centers text within a 64-character width |
| `LetterHelper` | Shuffles letters while preserving the key letter marker (`*`) |
| `ScoreHelper` | Fibonacci-based scoring: word length 3→1pt, 4→2pt, 5→3pt, 6→5pt, 7→8pt, 8→13pt, 9→21pt |

## Game Rules

- Words must be 3–9 letters long
- Words must contain the central (key) letter
- Words may not end in S (unless they end in SS)
- Each letter can only be used once per word
- Scoring uses Fibonacci numbers indexed by `(word.Length - 3)`

## Commands

All commands are prefixed with `:` — e.g. `:LETTERS`, `:WORDS`, `:MIX`, `:SCORE`, `:RESTART`, `:VERSION`, `:HELP`, `:EXIT`. Short forms exist (`:L`, `:W`, `:M`, `:S`, `:R`, `:V`, `:H`, `:X`).

## Dependencies

- **Target framework**: .NET 10
- **NuGet**: `AiAnnotations 1.0.0.9` (code provenance marking)
- **Runtime file**: `words.txt` (copied to output directory)

## Key Patterns

- Partial classes split logic (game engine) from presentation (display)
- Dependency injection via `IGameConsole` for testability
- Static helper classes for stateless utility functions
- `[ExcludeFromCodeCoverage]` on I/O-bound classes
- `[AiGenerated]` attributes to mark AI-assisted code
