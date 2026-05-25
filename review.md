# Code Review — WordWheelPlayer

## Summary

The solution is well-structured with good separation of concerns (partial classes, DI, helpers). The issues below range from potential runtime bugs to maintainability and performance improvements.

---

## Issues

### 1. Dictionary never cleared between restarts

**File:** `EnglishDictionary.cs` — `InitDictionary()`  
**Priority:** High  
**Description:** `candidateWords` is cleared at the start of `InitDictionary()`, but `englishDictionary` is not. Each time the player uses `:RESTART`, the entire word list is appended again to the existing list. Over multiple restarts this causes duplicate entries and growing memory usage. `WordIsInDictionary` still works (duplicates don't break `Contains`), but it's wasteful and could mask bugs.  
**Task:** Add `englishDictionary.Clear()` at the top of `InitDictionary()`.  
- [x] Completed

---

### 2. No feedback when a valid word is not in the dictionary

**File:** `GameEngine.cs` — `Start()` (around the `letterCount == word.Length` block)  
**Priority:** High  
**Description:** When a player enters a word that uses valid letters and passes all checks but is not found in the dictionary, nothing happens — no message is displayed. The game silently clears the screen and re-renders. The player has no idea why their word wasn't accepted.  
**Task:** Add a `DisplayMessage("Word not found in dictionary")` in the `else` branch when `WordIsInDictionary` returns false.  
- [x] Completed

---

### 3. Regex compiled on every line of the dictionary file

**File:** `EnglishDictionary.cs` — `InitDictionary()`  
**Priority:** Medium  
**Description:** `Regex.IsMatch(candidate, RegExPattern)` is called for every word in the file. The regex is recompiled each time. For a large dictionary file this is a noticeable startup cost.  
**Task:** Use a compiled `Regex` instance (e.g. `private static readonly Regex AlphaOnly = new("^[a-zA-Z]+$", RegexOptions.Compiled)`) or a .NET 7+ source generator `[GeneratedRegex]`.  
- [x] Completed — replaced with `[GeneratedRegex]` source generator

---

### 4. `FrontLoadCentreLetter` assumes exactly 9 letters and can index out of bounds

**File:** `Display.cs` — `FrontLoadCentreLetter()`  
**Priority:** Medium  
**Description:** The method uses a hardcoded index `gameLetters[8]` when the asterisk is at the end. If the string is shorter than 9 characters (e.g. due to a future difficulty mode or a bug), this will throw an `IndexOutOfRangeException`. Additionally, if the asterisk is at position 0, `asteriskIndex - 1` becomes -1.  
**Task:** Add guard clauses or derive indices from the string length rather than hardcoding. Consider validating input length.  
- [x] Completed — replaced hardcoded index with safe asterisk-relative logic and guard clause

---

### 5. `DisplayVersion` hardcodes a feature name string

**File:** `Display.cs` — `DisplayVersion()`  
**Priority:** Medium  
**Description:** The version display contains `const string Feature = "Fix looping hack"` — a stale developer note baked into the output. This will be shown to players and is meaningless to them.  
**Task:** Remove the hardcoded feature string or replace it with assembly version metadata (e.g. `Assembly.GetExecutingAssembly().GetName().Version`).  
- [x] Completed — replaced with assembly version display

---

### 6. `GameLetters` null check is redundant (property is never null)

**File:** `GameEngine.cs` — `Start()` (`:MIX` case and `InvalidLetterFound` call)  
**Priority:** Low  
**Description:** `GameLetters` is a non-nullable `string` property initialised to `string.Empty`. The `if (GameLetters != null)` checks are always true. This is dead code that adds confusion.  
**Task:** Remove the null checks. The property type already guarantees non-null.  
- [ ] To do

---

### 7. String concatenation in loops

**File:** `LetterHelper.cs` — `ShuffleLetters()`, `GameEngine.cs` — `Init()`  
**Priority:** Low  
**Description:** Both methods build strings character-by-character using `+=` in a loop. For a 9-character string this is negligible, but it's a poor pattern that a `StringBuilder` or `string.Join` would express more clearly.  
**Task:** Replace with `StringBuilder` or LINQ `string.Concat`/`string.Join` for clarity.  
- [ ] To do

---

### 8. `GameLetter.Letter` is `string?` but used as non-null everywhere

**File:** `GameLetter.cs`  
**Priority:** Low  
**Description:** `Letter` is declared as `string?` but is never set to null in practice. Consumers (e.g. `gameLetters.FirstOrDefault(x => x.Letter == ...)`) rely on it being non-null. This creates unnecessary nullable warnings or suppression.  
**Task:** Change to `public required string Letter { get; set; }` (or initialise with `= string.Empty`) to match actual usage.  
- [ ] To do

---

### 9. `EnglishDictionary.GameLetters` is a public field

**File:** `EnglishDictionary.cs`  
**Priority:** Low  
**Description:** `public List<string> GameLetters = []` is a mutable public field. This breaks encapsulation — callers can replace or mutate the list freely. The rest of the codebase uses properties.  
**Task:** Change to a property: `public List<string> GameLetters { get; set; } = []` or better, expose as `IReadOnlyList<string>`.  
- [ ] To do

---

### 10. `BestScoreHelper.CheckForBestScore` has a side effect that's not obvious from the name

**File:** `Helpers/BestScoreHelper.cs`  
**Priority:** Low  
**Description:** The method name suggests a read operation ("check"), but it also writes to disk when the score is higher. This is called from `DisplayBestTotals`, meaning simply viewing the score can trigger a file write. The coupling between display and persistence is surprising.  
**Task:** Rename to `UpdateBestScoreIfHigher` or separate the check from the save. Consider calling the save explicitly from the game loop rather than from a display method.  
- [ ] To do

---

### 11. Random word selection uses `Guid.NewGuid().ToString()` for ordering

**File:** `EnglishDictionary.cs` — `InitDictionary()`, `LetterHelper.cs` — `ShuffleLetters()`  
**Priority:** Low  
**Description:** Using `Guid.NewGuid().ToString()` as a sort key works but is unconventional and allocates strings unnecessarily. `Random.Shared.Next()` (or `Random.Shared.GetItems` in .NET 8+) is the idiomatic approach and avoids string allocations.  
**Task:** Replace GUID-based ordering with `Random.Shared` for shuffling and candidate selection.  
- [ ] To do

---

### 12. No unit test project

**File:** Solution-level  
**Priority:** Medium  
**Description:** The `IGameConsole` abstraction exists specifically to enable testing, but there is no test project in the solution. The scoring logic, dictionary validation, letter helpers, and game rules are all testable in isolation.  
**Task:** Add an xUnit/NUnit/MSTest project with tests covering at minimum: `ScoreHelper`, `LetterHelper`, `DisplayHelper`, `EnglishDictionary.WordIsInDictionary`, and the word validation logic in `GameEngine`.  
- [ ] To do

---

## Summary Table

| # | Issue | Priority | Status |
|---|-------|----------|--------|
| 1 | Dictionary not cleared on restart | High | ✅ Done |
| 2 | No feedback for invalid dictionary word | High | ✅ Done |
| 3 | Regex recompiled per line | Medium | ✅ Done |
| 4 | Hardcoded index in FrontLoadCentreLetter | Medium | ✅ Done |
| 5 | Stale feature string in version display | Medium | ✅ Done |
| 6 | Redundant null checks on GameLetters | Low | ⬜ To do |
| 7 | String concatenation in loops | Low | ⬜ To do |
| 8 | Nullable Letter property never null | Low | ⬜ To do |
| 9 | Public mutable field | Low | ⬜ To do |
| 10 | Side-effecting "check" method | Low | ⬜ To do |
| 11 | GUID-based randomisation | Low | ⬜ To do |
| 12 | No unit test project | Medium | ⬜ To do |
