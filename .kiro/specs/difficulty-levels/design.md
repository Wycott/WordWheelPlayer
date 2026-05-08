# Feature: Difficulty Levels

## Requirements

- [ ] Allow the player to select a difficulty level at game start
- [ ] Easy: 7-letter seed word (letters 3–7), more common key letter
- [ ] Normal: 9-letter seed word (letters 3–9) — current behaviour
- [ ] Hard: 9-letter seed word but less common key letter (avoid vowels)
- [ ] Adjust scoring multiplier by difficulty (Easy ×1, Normal ×1, Hard ×2)
- [ ] Display current difficulty in the header or via `:SCORE`

## Design

### Overview

Wraps the existing letter selection and scoring logic with a difficulty configuration. The core validation rules remain unchanged — difficulty affects seed word length, key letter selection bias, and score multiplier.

### Components Affected

| Component | Change |
|-----------|--------|
| `GameEngine.cs` | Accept difficulty parameter, pass to dictionary and scoring |
| `EnglishDictionary.cs` | Support variable max word length, adjust key letter weighting |
| `Display.cs` | Show difficulty in score display |
| `Helpers/ScoreHelper.cs` | Accept multiplier parameter |

### New Files

| File | Purpose |
|------|---------|
| `Difficulty.cs` | Enum or record defining difficulty presets |

### Data Flow

1. Player selects difficulty at start
2. `Difficulty` config determines max word length and letter weighting
3. `EnglishDictionary` uses max length to filter seed words
4. `ScoreHelper.CalculateWordScore` applies multiplier
5. Score display includes difficulty label

## Tasks

- [ ] Create `Difficulty` enum/record with Easy, Normal, Hard presets
- [ ] Add difficulty selection prompt at game start
- [ ] Pass max word length to `EnglishDictionary` constructor based on difficulty
- [ ] Adjust key letter selection weighting for Hard mode
- [ ] Add score multiplier to `ScoreHelper.CalculateWordScore`
- [ ] Display difficulty in score output
- [ ] Verify build passes
