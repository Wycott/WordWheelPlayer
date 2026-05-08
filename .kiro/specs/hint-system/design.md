# Feature: Hint System

## Requirements

- [ ] Add a `:HINT` command that reveals a clue about an unfound word
- [ ] Hints should reveal the first letter and length of a valid word not yet found
- [ ] Limit the number of hints per game (default 3)
- [ ] Each hint used deducts points from the score (e.g. -2 per hint)
- [ ] Display remaining hints when the command is used

## Design

### Overview

Provides a structured hint mechanism that helps players who are stuck without fully revealing answers. The existing `:PEEK` easter egg reveals the 9-letter word directly — this feature is a more balanced, visible alternative for any valid word.

### Components Affected

| Component | Change |
|-----------|--------|
| `GameEngine.cs` | Add hint state (count remaining), find unfound words, apply penalty |
| `Display.cs` | Add hint display method showing clue and remaining hints |
| `EnglishDictionary.cs` | Add method to return valid words for current letters (for hint selection) |

### New Files

| File | Purpose |
|------|---------|
| `Helpers/HintHelper.cs` | Logic for selecting a hint word and formatting the clue |

### Data Flow

1. Player types `:HINT`
2. `HintHelper` finds a valid word not in `wordsFoundSoFar`
3. Returns first letter + word length as clue (e.g. "S _ _ _ _ (5 letters)")
4. Deducts penalty from score, decrements remaining hints
5. If no hints remain, displays a message

## Tasks

- [ ] Create `HintHelper` with word selection and clue formatting
- [ ] Add hint counter and penalty constants to `GameEngine`
- [ ] Add `:HINT` / `:HI` command handling in the game loop
- [ ] Add display method for hint output
- [ ] Ensure hints only suggest words that use valid letters and key letter
- [ ] Verify build passes
