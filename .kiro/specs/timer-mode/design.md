# Feature: Timer / Countdown Mode

## Requirements

- [ ] Add an optional timed mode where the player has a configurable time limit (default 2 minutes)
- [ ] Display remaining time periodically (e.g. when a word is submitted)
- [ ] End the game automatically when time expires
- [ ] Show final score summary when time runs out
- [ ] Allow the player to choose timed or untimed mode at game start

## Design

### Overview

Introduces a countdown timer that constrains the game session. The timer starts when the first letter wheel is displayed and ends the input loop when expired. This adds urgency and replayability without changing the core word validation logic.

### Components Affected

| Component | Change |
|-----------|--------|
| `GameEngine.cs` | Add timer state, check expiry in game loop, prompt for mode selection |
| `Display.cs` | Add time-remaining display method |
| `IGameConsole.cs` | Potentially add `KeyAvailable` check for non-blocking input |
| `GameConsole.cs` | Implement `KeyAvailable` |

### New Files

| File | Purpose |
|------|---------|
| `Helpers/TimerHelper.cs` | Encapsulates countdown logic and formatting |

### Data Flow

1. Player selects timed/untimed at start
2. `TimerHelper` records start time and duration
3. Each iteration of the game loop checks `TimerHelper.IsExpired`
4. When expired, loop exits and score summary is displayed

## Tasks

- [ ] Create `TimerHelper` with start, remaining, and expired logic
- [ ] Add mode selection prompt before game starts
- [ ] Integrate timer check into the game loop in `GameEngine.Start()`
- [ ] Add `:TIME` command to display remaining time on demand
- [ ] Display final summary on expiry
- [ ] Verify build passes
