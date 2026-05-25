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
- [x] Completed — removed both redundant null checks

---

### 7. String concatenation in loops

**File:** `LetterHelper.cs` — `ShuffleLetters()`, `GameEngine.cs` — `Init()`  
**Priority:** Low  
**Description:** Both methods build strings character-by-character using `+=` in a loop. For a 9-character string this is negligible, but it's a poor pattern that a `StringBuilder` or `string.Join` would express more clearly.  
**Task:** Replace with `StringBuilder` or LINQ `string.Concat`/`string.Join` for clarity.  
- [x] Completed — replaced with `StringBuilder` in both `LetterHelper` and `GameEngine.Init()`

---

### 8. `GameLetter.Letter` is `string?` but used as non-null everywhere

**File:** `GameLetter.cs`  
**Priority:** Low  
**Description:** `Letter` is declared as `string?` but is never set to null in practice. Consumers (e.g. `gameLetters.FirstOrDefault(x => x.Letter == ...)`) rely on it being non-null. This creates unnecessary nullable warnings or suppression.  
**Task:** Change to `public required string Letter { get; set; }` (or initialise with `= string.Empty`) to match actual usage.  
- [x] Completed — changed to `public required string Letter { get; set; }`

---

### 9. `EnglishDictionary.GameLetters` is a public field

**File:** `EnglishDictionary.cs`  
**Priority:** Low  
**Description:** `public List<string> GameLetters = []` is a mutable public field. This breaks encapsulation — callers can replace or mutate the list freely. The rest of the codebase uses properties.  
**Task:** Change to a property: `public List<string> GameLetters { get; set; } = []` or better, expose as `IReadOnlyList<string>`.  
- [x] Completed — changed to property `public List<string> GameLetters { get; set; } = []`

---

### 10. `BestScoreHelper.CheckForBestScore` has a side effect that's not obvious from the name

**File:** `Helpers/BestScoreHelper.cs`  
**Priority:** Low  
**Description:** The method name suggests a read operation ("check"), but it also writes to disk when the score is higher. This is called from `DisplayBestTotals`, meaning simply viewing the score can trigger a file write. The coupling between display and persistence is surprising.  
**Task:** Rename to `UpdateBestScoreIfHigher` or separate the check from the save. Consider calling the save explicitly from the game loop rather than from a display method.  
- [x] Completed — renamed to `UpdateBestScoreIfHigher`

---

### 11. Random word selection uses `Guid.NewGuid().ToString()` for ordering

**File:** `EnglishDictionary.cs` — `InitDictionary()`, `LetterHelper.cs` — `ShuffleLetters()`  
**Priority:** Low  
**Description:** Using `Guid.NewGuid().ToString()` as a sort key works but is unconventional and allocates strings unnecessarily. `Random.Shared.Next()` (or `Random.Shared.GetItems` in .NET 8+) is the idiomatic approach and avoids string allocations.  
**Task:** Replace GUID-based ordering with `Random.Shared` for shuffling and candidate selection.  
- [x] Completed — replaced with `Random.Shared.Next()` in both files

---

### 12. No unit test project

**File:** Solution-level  
**Priority:** Medium  
**Description:** The `IGameConsole` abstraction exists specifically to enable testing, but there is no test project in the solution. The scoring logic, dictionary validation, letter helpers, and game rules are all testable in isolation.  
**Task:** Add an xUnit/NUnit/MSTest project with tests covering at minimum: `ScoreHelper`, `LetterHelper`, `DisplayHelper`, `EnglishDictionary.WordIsInDictionary`, and the word validation logic in `GameEngine`.  
- [x] Completed — test project already exists (`WordWheelPlayer.Test`, 73 passing tests). Issue was raised in error.

---

## Summary Table

| # | Issue | Priority | Status |
|---|-------|----------|--------|
| 1 | Dictionary not cleared on restart | High | ✅ Done |
| 2 | No feedback for invalid dictionary word | High | ✅ Done |
| 3 | Regex recompiled per line | Medium | ✅ Done |
| 4 | Hardcoded index in FrontLoadCentreLetter | Medium | ✅ Done |
| 5 | Stale feature string in version display | Medium | ✅ Done |
| 6 | Redundant null checks on GameLetters | Low | ✅ Done |
| 7 | String concatenation in loops | Low | ✅ Done |
| 8 | Nullable Letter property never null | Low | ✅ Done |
| 9 | Public mutable field | Low | ✅ Done |
| 10 | Side-effecting "check" method | Low | ✅ Done |
| 11 | GUID-based randomisation | Low | ✅ Done |
| 12 | No unit test project | Medium | ✅ Done (already existed) |

---

## Final Review (Post-Fix)

After addressing all 12 original issues, a final pass identified the following remaining items:

### 13. `InvalidLetterFound` checks against `GameLetters` which contains the `*` marker

**File:** `GameEngine.cs` — `InvalidLetterFound()` / `Start()`  
**Priority:** Low  
**Description:** The `GameLetters` string contains the `*` character as a key-letter marker. `InvalidLetterFound` checks whether each character in the guessed word exists in `GameLetters`. Since `*` is never typed by the player, this doesn't cause a false positive — but it means the validation is checking against a string that includes a non-letter character. If a player somehow entered `*`, it would pass this check. Functionally harmless but semantically imprecise.  
**Task:** Strip `*` from `GameLetters` before passing to `InvalidLetterFound`, or use the `gameLetters` list (which has clean letter values) for validation instead.  
- [ ] To do

---

### 14. `ScoreHelper.CalculateWordScore` can throw `IndexOutOfRangeException` for words longer than 9 letters

**File:** `Helpers/ScoreHelper.cs`  
**Priority:** Low  
**Description:** The `FibonacciNumbers` list has 7 entries (indices 0–6, covering word lengths 3–9). If a word longer than 9 characters were passed in, `word.Length - 3` would exceed the list bounds. Currently the game caps words at 9 letters so this can't happen in practice, but the method is public and has no guard.  
**Task:** Add a bounds check: if `index >= FibonacciNumbers.Count` return the last value or throw a meaningful exception.  
- [ ] To do

---

### 15. `DisplayVersion` will show a null/empty version when running via `dotnet run`

**File:** `Display.cs` — `DisplayVersion()`  
**Priority:** Low  
**Description:** `Assembly.GetExecutingAssembly().GetName().Version` returns `1.0.0.0` by default unless a `<Version>` or `<AssemblyVersion>` is set in the `.csproj`. Additionally, `Assembly.GetExecutingAssembly().Location` returns an empty string for single-file published apps, which would make `File.GetLastWriteTime` return `01/01/1601`. Not a crash, but confusing output.  
**Task:** Set a `<Version>` in the `.csproj` and add a fallback for empty `Location`.  
- [ ] To do

---

### 16. `wordsFoundSoFar.Sort()` runs on every loop iteration including when no word was added

**File:** `GameEngine.cs` — `Start()`  
**Priority:** Low  
**Description:** After every input (including commands that `continue` past the sort), the code at the bottom of the loop sorts the word list, clears the screen, and re-renders. For commands that already `continue`, this is fine — they skip it. But for invalid words (e.g. "not in dictionary"), the screen is still cleared and re-rendered even though nothing changed. This causes the error message to flash briefly before being cleared.  
**Task:** Move the clear/re-render block so it only executes when a word is successfully added, or `continue` after displaying the "not in dictionary" message.  
- [ ] To do

---

### 17. `EnglishDictionary.LongestWord` and `GameLetters` are publicly settable

**File:** `EnglishDictionary.cs`  
**Priority:** Low  
**Description:** Both `LongestWord` and `GameLetters` have public setters but should only be set internally by `InitDictionary()`. External code could overwrite them and break game state.  
**Task:** Change to `public string? LongestWord { get; private set; }` and `public List<string> GameLetters { get; private set; } = []`.  
- [ ] To do

---

### Summary Table (Final Review)

| # | Issue | Priority | Status |
|---|-------|----------|--------|
| 13 | `*` marker in letter validation string | Low | ⬜ To do |
| 14 | No bounds check in ScoreHelper | Low | ⬜ To do |
| 15 | Version display may show defaults | Low | ⬜ To do |
| 16 | Screen clears after failed word attempts | Low | ⬜ To do |
| 17 | Publicly settable properties on EnglishDictionary | Low | ⬜ To do |
