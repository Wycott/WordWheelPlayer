# Feature: Statistics & History Tracking

## Requirements

- [ ] Track game history across sessions (not just best score)
- [ ] Record: date, score, words found count, time played, seed word
- [ ] Add a `:STATS` command to display lifetime statistics
- [ ] Show: total games played, average score, total words found, longest streak
- [ ] Persist statistics to a JSON file

## Design

### Overview

Extends the existing `BestGame.json` persistence pattern to track a richer history of all games played. This gives players a sense of progression beyond just the single best score.

### Components Affected

| Component | Change |
|-----------|--------|
| `GameEngine.cs` | Record game result on exit/restart, add `:STATS` command |
| `Display.cs` | Add statistics display method |
| `Helpers/BestScoreHelper.cs` | Optionally refactor to share persistence patterns |

### New Files

| File | Purpose |
|------|---------|
| `GameResult.cs` | Model for a single game session result |
| `GameHistory.cs` | Model for the collection of all results + computed stats |
| `Helpers/StatsHelper.cs` | Load/save/compute statistics from history file |

### Persistence Format

```json
{
  "games": [
    {
      "date": "2026-05-08T14:30:00",
      "score": 42,
      "wordsFound": 12,
      "seedWord": "EDUCATION",
      "durationSeconds": 180
    }
  ]
}
```

### Data Flow

1. On game end (exit or restart), `GameEngine` creates a `GameResult`
2. `StatsHelper` appends it to the history file
3. `:STATS` command loads history and computes aggregates
4. Display renders the statistics summary

## Tasks

- [ ] Create `GameResult` and `GameHistory` models
- [ ] Create `StatsHelper` with load, save, and compute methods
- [ ] Record game result when exiting or restarting
- [ ] Add `:STATS` / `:ST` command to game loop
- [ ] Add statistics display method in `Display.cs`
- [ ] Verify build passes
