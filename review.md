# Code Review — WordWheelPlayer

## Summary

The codebase is well-structured with clean separation of concerns, good use of dependency injection for testability, and modern C# features. The build is clean (no warnings) and all 73 tests pass. Six issues were identified — no high-priority bugs, three medium concerns around robustness and test hygiene, and three low-priority improvements.

---

## Issues

### 1. `InvalidLetterFound` validates against `GameLetters` which contains the `*` marker

**File:** `GameEngine.cs` — `InvalidLetterFound()` / `Start()`
**Priority:** Medium
**Description:** The `GameLetters` string includes the `*` character as a key-letter positional marker. `InvalidLetterFound` checks whether each character in the guessed word exists in `GameLetters`. If a player typed `*` it would pass this validation check — the character wouldn't be rejected as invalid. While unlikely in practice, it means letter validation is semantically imprecise and could mask bugs if the marker character ever changes.
**Task:** Strip `*` from `GameLetters` before passing to `InvalidLetterFound`, or validate against the `gameLetters` list (which has clean letter values) instead.
- [ ] To do

---

### 2. `ScoreHelper.CalculateWordScore` can throw `IndexOutOfRangeException` for words longer than 9 letters

**File:** `Helpers/ScoreHelper.cs` — `CalculateWordScore()`
**Priority:** Medium
**Description:** The `FibonacciNumbers` list has 7 entries (indices 0–6, covering word lengths 3–9). The method is `public` and has no upper-bounds guard. If called with a word longer than 9 characters, `word.Length - 3` would exceed the list bounds and throw. The game currently caps input at 9 letters so this can't happen via normal play, but the method's public API contract doesn't enforce that.
**Task:** Add a bounds check — clamp the index to `FibonacciNumbers.Count - 1` or return 0 for out-of-range lengths.
- [ ] To do

---

### 3. TODO and commented-out test left in test project

**File:** `WordWheelPlayer.Test/GameEngineTests.cs` — line 190
**Priority:** Medium
**Description:** A commented-out test `Start_ShouldRejectWordEndingInS` is preceded by `// TODO: Write a proper test here`. Commented-out code and unresolved TODOs add noise. The message in the commented test (`"Words may not end in S - sorry!"`) also doesn't match the current production message (`"Words may not end in S (-SS is allowed)"`), suggesting this was never completed after the message changed.
**Task:** Either implement the test with the correct assertion or remove the commented block entirely.
- [ ] To do

---

### 4. `DisplayVersion` will show a default version and a bogus build date in some scenarios

**File:** `Display.cs` — `DisplayVersion()`
**Priority:** Low
**Description:** No `<Version>` is set in the `.csproj`, so `Assembly.GetExecutingAssembly().GetName().Version` returns `1.0.0.0`. Additionally, `Assembly.GetExecutingAssembly().Location` returns an empty string for single-file published apps, which would make `File.GetLastWriteTime("")` return `01/01/1601`. Neither crashes the app but produces confusing output.
**Task:** Add a `<Version>` property in the `.csproj` and add a fallback for when `Location` is empty.
- [ ] To do

---

### 5. `EnglishDictionary.LongestWord` and `GameLetters` have public setters

**File:** `EnglishDictionary.cs`
**Priority:** Low
**Description:** Both properties are only set internally by `InitDictionary()`. The public setters allow external code to overwrite game state, breaking encapsulation. The `GameLetters` property setter was noted as a legacy exception in the project overview, but `LongestWord` has no such justification.
**Task:** Change to `private set` on both properties (or at minimum on `LongestWord`).
- [ ] To do

---

### 6. `DisplayWordsFound` builds column lines with string concatenation in a loop

**File:** `Display.cs` — `DisplayWordsFound()`
**Priority:** Low
**Description:** The inner loop uses `line += wordsFoundSoFar[i + col].PadRight(columnWidth)` to build each row. For a small word list this is negligible, but it's an avoidable allocation pattern when `string.Concat` or `StringBuilder` would be clearer and consistent with the rest of the codebase (which already migrated away from `+=` loops).
**Task:** Replace with `StringBuilder` or `string.Join` for consistency.
- [ ] To do

---

## Summary Table

| # | Issue | Priority | Status |
|---|-------|----------|--------|
| 1 | `*` marker in letter validation string | Medium | ⬜ To do |
| 2 | No bounds check in ScoreHelper | Medium | ⬜ To do |
| 3 | TODO and commented-out test in test project | Medium | ⬜ To do |
| 4 | Version display may show defaults | Low | ⬜ To do |
| 5 | Publicly settable properties on EnglishDictionary | Low | ⬜ To do |
| 6 | String concatenation in DisplayWordsFound loop | Low | ⬜ To do |
